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

        public IActionResult AnalysisHistory()
        {
            return View();
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

