using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.ORM;

public class DefaultContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Branch> Branchs { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Cart> Carts { get; set; }

    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var moneyConverter = new MoneyConverter();

        modelBuilder.Entity<CartItem>()
            .Property(e => e.UnitProductItemPrice)
            .HasConversion(moneyConverter);

        modelBuilder.Entity<CartItem>()
            .Property(e => e.DiscountAmount)
            .HasConversion(moneyConverter);

        modelBuilder.Entity<CartItem>()
            .Property(e => e.TotalSaleItemPrice)
            .HasConversion(moneyConverter);

        modelBuilder.Entity<CartItem>()
            .Property(e => e.TotalWithoutDiscount)
            .HasConversion(moneyConverter);

        modelBuilder.Entity<Cart>()
            .Property(e => e.TotalSalePrice)
            .HasConversion(moneyConverter);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
public class YourDbContextFactory : IDesignTimeDbContextFactory<DefaultContext>
{
    public DefaultContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DefaultContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(
               connectionString,
               b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
        );

        return new DefaultContext(builder.Options);
    }
}