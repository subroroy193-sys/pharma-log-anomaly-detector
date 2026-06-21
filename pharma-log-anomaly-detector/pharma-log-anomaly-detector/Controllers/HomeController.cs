using Microsoft.AspNetCore.Mvc;
using pharma_log_anomaly_detector.Data;
using pharma_log_anomaly_detector.Models;
using System.Diagnostics;

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
        public async Task<IActionResult> UploadLog([FromForm(Name = "csvFile")] IFormFile? csvFile)
        {
            var uploadedFile = csvFile ?? Request.Form.Files.FirstOrDefault();

            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                ViewBag.Message = "Please select a CSV file.";
                return View();
            }

            var extension = Path.GetExtension(uploadedFile.FileName).ToLowerInvariant();
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

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(uploadedFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

            var logFile = new LogFile
            {
                FileName = uploadedFile.FileName,
                FilePath = filePath,
                UploadDate = DateTime.Now,
                UploadedBy = 0,
                RecordCount = 0,
                FileStatus = "Uploaded",
                AnalysisStatus = "Pending"
            };

            _context.LogFiles.Add(logFile);
            await _context.SaveChangesAsync();

            ViewBag.Message = "File uploaded successfully.";
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

