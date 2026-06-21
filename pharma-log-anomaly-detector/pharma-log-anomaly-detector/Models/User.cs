using System;

namespace pharma_log_anomaly_detector.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; }
    }
}


