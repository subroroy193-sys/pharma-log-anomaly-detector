using System;
using System.Collections.Generic;

namespace pharma_log_anomaly_detector.Models
{
    public class LogFile
    {
        public int LogFileId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public DateTime UploadDate { get; set; } = DateTime.Now;

        public int UploadedBy { get; set; }

        public int RecordCount { get; set; }

        public string FileStatus { get; set; } = "Uploaded";

        public string AnalysisStatus { get; set; } = "Pending";
    }
}


