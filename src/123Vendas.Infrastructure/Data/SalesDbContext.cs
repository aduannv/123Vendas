using Microsoft.EntityFrameworkCore;
using _123Vendas.Domain.Entities;

namespace _123Vendas.Infrastructure.Data;

public class SalesDbContext(DbContextOptions<SalesDbContext> options) : DbContext(options)
{
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("sales");
            entity.HasKey(s => s.Id);

            entity.Property(s => s.SaleDate)
                .IsRequired();

            entity.Property(s => s.CustomerId)
                .IsRequired();

            entity.Property(s => s.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.BranchId)
                .IsRequired();

            entity.Property(s => s.BranchName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.ToTable("saleitems");
            entity.HasKey(si => si.Id);

            entity.Property(si => si.ProductId)
                .IsRequired();

            entity.Property(si => si.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(si => si.Quantity)
                .IsRequired();

            entity.Property(si => si.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); 

            entity.Property(si => si.Discount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");            
        });
    }
}
