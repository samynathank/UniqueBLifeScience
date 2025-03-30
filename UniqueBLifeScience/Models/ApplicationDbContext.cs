using Microsoft.EntityFrameworkCore;

namespace UniqueBLifeScience.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
               : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<Stocks> Stocks { get; set; }

        public DbSet<StockSub> StockSub { get; set; }

        public DbSet<Sales> Sales { get; set; }

        public DbSet<SalesSub> SalesSub { get; set; }

        public DbSet<OwnTable> OwnTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasKey(u => u.UserID);
            //modelBuilder.Entity<Products>();
            modelBuilder.Entity<Stocks>()
            .HasMany(s => s.StockSub)
            .WithOne(ss => ss.Stock)
            .HasForeignKey(ss => ss.StockID);
            modelBuilder.Entity<Sales>()
            .HasMany(s => s.SalesSub)
            .WithOne(ss => ss.Sales)
            .HasForeignKey(ss => ss.SalesID);
        }
    }
}
