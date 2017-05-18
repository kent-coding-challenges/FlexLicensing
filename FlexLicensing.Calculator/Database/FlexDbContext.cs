using System.Data.Entity;
using FlexLicensing.Calculator.Models;

namespace FlexLicensing.Calculator.Database
{
    /// <summary>
    ///     EF database context for FlexLicensing project.
    /// </summary>
    public class FlexDbContext : DbContext
    {
        public DbSet<InstallLog> InstallLogs { get; set; }

        /// <summary>
        ///     Default constructor to initialize context name.
        /// </summary>
        public FlexDbContext() : base("FlexDbContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}