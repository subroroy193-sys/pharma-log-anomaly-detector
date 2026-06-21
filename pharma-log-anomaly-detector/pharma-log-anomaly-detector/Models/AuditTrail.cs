using System;

namespace pharma_log_anomaly_detector.Models
{
    public class AuditTrail
    {
        public long AuditTrailId { get; set; }

        public int UserId { get; set; }

        public string ActionType { get; set; } = string.Empty;

        public string ActionDescription { get; set; } = string.Empty;

        public DateTime ActionDate { get; set; } = DateTime.Now;

        public string? IPAddress { get; set; }
    }
}
