using System;
using System.Collections.Generic;

namespace pharma_log_anomaly_detector.Models
{
    public class LogEntry
    {
        public long LogEntryId { get; set; }

        public int FileId { get; set; }

        public DateTime TimeStamp { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Module { get; set; } = string.Empty;

        public string EventType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
        public int? DurationMs { get; set; }
        public string? IPAddress { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}


