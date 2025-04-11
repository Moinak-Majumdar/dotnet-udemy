using hello_api.models;
using Microsoft.EntityFrameworkCore;

namespace hello_api.context
{
    public class EFContext(IConfiguration config) : DbContext
    {
        private readonly string _db_con = config.GetConnectionString("DefaultConnection");

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<JobInfo> JobInfo { get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"), optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<User> ()
                .ToTable("TblUsers", "dbo")
                .HasKey(u => u.UserId);

            modelBuilder.Entity<JobInfo>()
                .ToTable("TblJobInfo", "dbo")
                .HasKey(j => j.UserId);

        }

    }
}