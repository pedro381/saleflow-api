using Microsoft.EntityFrameworkCore;
using SaleFlow.Domain.Entities;

namespace SaleFlow.Repository.Data
{
    public class SaleFlowDbContext : DbContext
    {
        public SaleFlowDbContext(DbContextOptions<SaleFlowDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; } = default!;
        public DbSet<SaleItem> SaleItems { get; set; } = default!;
        public DbSet<Customer> Customers { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>()
                .HasMany(e => e.SaleItems)
                .WithOne()
                .HasForeignKey(o => o.SaleItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<Sale>()
                .Ignore(si => si.TotalAmount);

            modelBuilder.Entity<SaleItem>()
                .Property(x => x.Discount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
