
using CustomerLeadImages.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerLeadImages.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<ProfileImage> ProfileImages => Set<ProfileImage>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProfileImage>().HasIndex(x => new { x.OwnerType, x.OwnerId });
        base.OnModelCreating(modelBuilder);
    }
}
public static class Seed
{
    public static void Ensure(AppDbContext db)
    {
        if (!db.Customers.Any())
        {
            db.Customers.AddRange(new Customer { Name = "Acme" }, new Customer { Name = "Globex" });
        }
        if (!db.Leads.Any())
        {
            db.Leads.AddRange(new Lead { Company = "Initech" }, new Lead { Company = "Umbrella" });
        }
        db.SaveChanges();
    }
}
