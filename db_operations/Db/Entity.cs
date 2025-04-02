using db_operations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace db_operations.Db
{
    public class Entity(IConfiguration c) : DbContext
    {
        private string _db_uri = c.GetConnectionString("default");

        public DbSet<Computer>? Computers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            if (!option.IsConfigured)
            {
                option.UseSqlServer(_db_uri, options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<Computer>().HasKey(k => k.ComputerId);
            modelBuilder.Entity<Computer>().ToTable("TblComputer", "dbo");
        }
    }
}