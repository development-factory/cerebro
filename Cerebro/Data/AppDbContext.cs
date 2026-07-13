using Microsoft.EntityFrameworkCore;

namespace Cerebro.Data;

public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}
