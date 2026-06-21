using Microsoft.AspNetCore.Mvc;
using pharma_log_anomaly_detector.Models;
using System.Diagnostics;

namespace pharma_log_anomaly_detector.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UploadLog()
        {
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
