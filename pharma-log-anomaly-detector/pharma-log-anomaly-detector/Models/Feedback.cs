using System;

namespace pharma_log_anomaly_detector.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        public int AnomalyId { get; set; }

        public int UserId { get; set; }

        public string FeedbackType { get; set; } = string.Empty;

        public string? Comment { get; set; }

        public DateTime FeedbackDate { get; set; } = DateTime.Now;
    }
}
