using System;

namespace pharma_log_anomaly_detector.Models
{
    public class CsvLogRecord
    {
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int? DurationMs { get; set; }
        public string? IPAddress { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
