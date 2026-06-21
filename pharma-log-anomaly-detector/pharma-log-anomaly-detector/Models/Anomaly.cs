using System;

namespace pharma_log_anomaly_detector.Models
{
    public class Anomaly
    {
        public int AnomalyId { get; set; }

        public long LogId { get; set; }

        public decimal AnomalyScore { get; set; }

        public string Severity { get; set; } = string.Empty;

        public string RootCause { get; set; } = string.Empty;

        public DateTime DetectionDate { get; set; } = DateTime.Now;

        public bool IsConfirmed { get; set; } = false;
    }
}