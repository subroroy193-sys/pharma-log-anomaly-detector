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
    }
}