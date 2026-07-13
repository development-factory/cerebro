using Microsoft.EntityFrameworkCore;

namespace Cerebro.Data;

public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Invoice>()
            .HasMany<InvoiceLineItem>()
            .WithOne(lineItem => lineItem.Invoice)
            .HasForeignKey(lineItem => lineItem.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InvoiceLineItem>()
            .HasOne(lineItem => lineItem.CatalogItem)
            .WithMany()
            .HasForeignKey(lineItem => lineItem.CatalogItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
