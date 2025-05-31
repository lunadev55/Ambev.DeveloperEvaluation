using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.ORM;

public class DefaultContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<Product> Products { get; set; }

    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        // --- Sale mapping ---
        modelBuilder.Entity<Sale>(eb =>
        {
            eb.ToTable("Sales");
            eb.HasKey(s => s.Id);

            eb.Property(s => s.SaleNumber).IsRequired();
            eb.Property(s => s.Date).IsRequired();
            eb.Property(s => s.IsCancelled)
              .HasColumnName("IsCancelled")
              .HasDefaultValue(false);
                        
            eb.OwnsOne(s => s.CustomerId, cb =>
            {
                cb.Property(c => c.Value)
                  .HasColumnName("CustomerId")
                  .IsRequired();
            });
                        
            eb.OwnsOne(s => s.BranchId, bb =>
            {
                bb.Property(b => b.Value)
                  .HasColumnName("BranchId")
                  .IsRequired();
            });

            eb.OwnsMany(s => s.Items, ib =>
            {
                ib.ToTable("SaleItems");
                ib.HasKey(i => i.Id);
                ib.Property(i => i.ProductId).IsRequired();
                ib.Property(i => i.Quantity).IsRequired();
                ib.Property(i => i.UnitPrice)
                  .HasColumnType("decimal(18,2)");
                ib.Property(i => i.DiscountRate)
                  .HasColumnType("decimal(5,2)");
                ib.Property<bool>("IsCancelled")
                  .HasColumnName("IsCancelled")
                  .HasDefaultValue(false);
            });
        });

        // Tell EF that Rating is owned by Product and should be stored as columns in Products table
        modelBuilder.Entity<Product>()
            .OwnsOne(
                p => p.Rating,
                r =>
                {
                    // Optional: rename the Rating properties into separate columns
                    r.Property(x => x.Rate)
                        .HasColumnName("RatingRate")
                        .IsRequired();

                    r.Property(x => x.Count)
                        .HasColumnName("RatingCount")
                        .IsRequired();
                }
            );
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

        // code updated to add Migrations to Amber.Developerevaluation.ORM project
        builder.UseNpgsql(
               connectionString,
               b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")               
        );

        return new DefaultContext(builder.Options);
    }
}