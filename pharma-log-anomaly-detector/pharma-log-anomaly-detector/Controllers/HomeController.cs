using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using pharma_log_anomaly_detector.Data;
using pharma_log_anomaly_detector.Models;
using System.Diagnostics;
using System.Globalization;


namespace pharma_log_anomaly_detector.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadLog()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadLog(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ViewBag.Message = "Please select a CSV file.";
                return View();
            }

            var extension = Path.GetExtension(csvFile.FileName).ToLowerInvariant();
            if (extension != ".csv")
            {
                ViewBag.Message = "Only CSV files are allowed.";
                return View();
            }

            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(csvFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await csvFile.CopyToAsync(stream);
            }

            var logFile = new LogFile
            {
                FileName = csvFile.FileName,
                FilePath = filePath,
                UploadDate = DateTime.Now,
                UploadedBy = 0,
                RecordCount = 0,
                FileStatus = "Uploaded",
                AnalysisStatus = "Pending"
            };

            _context.LogFiles.Add(logFile);
            await _context.SaveChangesAsync();

            int recordCount = 0;

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<CsvLogRecord>().ToList();

                foreach (var row in rows)
                {
                    var entry = new LogEntry
                    {
                        FileId = logFile.LogFileId,
                        TimeStamp = row.Timestamp,
                        UserName = row.UserName,
                        Module = row.Module,
                        EventType = row.EventType,
                        Status = row.Status,
                        DurationMs = row.DurationMs,
                        IPAddress = row.IPAddress,
                        Message = row.Message
                    };

                    _context.LogEntries.Add(entry);
                    recordCount++;
                }
            }

            logFile.RecordCount = recordCount;
            _context.LogFiles.Update(logFile);
            await _context.SaveChangesAsync();

            ViewBag.Message = $"File uploaded successfully. {recordCount} log entries saved.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RunAnalysis(int id)
        {
            var file = await _context.LogFiles.FindAsync(id);

            if (file == null)
            {
                return NotFound();
            }

            var logs = _context.LogEntries
                .Where(l => l.FileId == id)
                .OrderBy(l => l.TimeStamp)
                .ToList();

            var analysis = new AnalysisRun
            {
                FileId = file.LogFileId,
                StartedBy = 0,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                TotalLogs = logs.Count,
                TotalAnomalies = 0,
                Status = "Completed"
            };

            var anomalies = new List<Anomaly>();

            foreach (var log in logs)
            {
                string severity = string.Empty;
                string rootCause = string.Empty;
                double score = 0;

                var isFailure = log.Status.Equals("Failure", StringComparison.OrdinalIgnoreCase);
                var highDuration = log.DurationMs.HasValue && log.DurationMs.Value > 5000;
                var mediumDuration = log.DurationMs.HasValue && log.DurationMs.Value > 3000;

                if (highDuration && isFailure)
                {
                    severity = "Critical";
                    rootCause = "High execution time with failure status";
                    score = 0.95;
                }
                else if (highDuration)
                {
                    severity = "Major";
                    rootCause = "Execution time exceeded threshold";
                    score = 0.80;
                }
                else if (isFailure)
                {
                    severity = "Minor";
                    rootCause = "Operation returned failure status";
                    score = 0.60;
                }

                if (!string.IsNullOrEmpty(severity))
                {
                    anomalies.Add(new Anomaly
                    {
                        LogId = log.LogEntryId,
                        AnomalyScore = score,
                        Severity = severity,
                        RootCause = rootCause,
                        DetectionDate = DateTime.Now,
                        IsConfirmed = false
                    });
                }
            }

            analysis.TotalAnomalies = anomalies.Count;

            file.AnalysisStatus = "Completed";

            _context.AnalysisRuns.Add(analysis);
            _context.Anomalies.AddRange(anomalies);
            _context.LogFiles.Update(file);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AnalysisHistory));
        }



        public IActionResult AnalysisHistory()
        {
            var files = _context.LogFiles
            .OrderByDescending(f => f.UploadDate)
            .ToList();

            return View(files);
        }

        public IActionResult Reports()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

