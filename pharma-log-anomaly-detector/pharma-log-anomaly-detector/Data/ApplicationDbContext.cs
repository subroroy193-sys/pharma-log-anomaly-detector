using Microsoft.EntityFrameworkCore;
using pharma_log_anomaly_detector.Models;

namespace pharma_log_anomaly_detector.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LogFile> LogFiles { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<AnalysisRun> AnalysisRuns { get; set; }
        public DbSet<Anomaly> Anomalies { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }

    }
}