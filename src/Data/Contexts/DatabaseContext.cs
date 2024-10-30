using Microsoft.EntityFrameworkCore;
using ReciclagemApi.Models;

namespace ReciclagemApi.Data.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<MaterialModel> Materials { get; set; }
        public DbSet<RecyclingReportModel> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserModel>().ToTable("tb_users");
            modelBuilder.Entity<MaterialModel>().ToTable("tb_materials");
            modelBuilder.Entity<RecyclingReportModel>().ToTable("tb_reports");
        }
    }
}
