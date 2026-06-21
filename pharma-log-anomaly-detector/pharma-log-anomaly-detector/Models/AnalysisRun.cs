using System;

namespace pharma_log_anomaly_detector.Models
{
    public class AnalysisRun
    {
        public int AnalysisRunId { get; set; }

        public int FileId { get; set; }

        public int StartedBy { get; set; }

        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime? EndTime { get; set; }

        public int TotalLogs { get; set; }

        public int TotalAnomalies { get; set; }

        public string Status { get; set; } = "Pending";
    }
}


