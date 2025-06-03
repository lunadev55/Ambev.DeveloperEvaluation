using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.ORM
{
    /// <summary>
    /// Represents the Entity Framework Core database context for the Developer Evaluation application.
    /// Contains DbSet properties for Users, Sales, SaleItems, and Products.
    /// </summary>
    public class DefaultContext : DbContext
    {
        /// <summary>
        /// Gets or sets the Users DbSet.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the Sales DbSet.
        /// </summary>
        public DbSet<Sale> Sales { get; set; }

        /// <summary>
        /// Gets or sets the SaleItems DbSet.
        /// </summary>
        public DbSet<SaleItem> SaleItems { get; set; }

        /// <summary>
        /// Gets or sets the Products DbSet.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options to configure the context.</param>
        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configures the model by applying entity configurations and mapping owned types.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            // --- Sale mapping ---
            modelBuilder.Entity<Sale>(eb =>
            {
                eb.ToTable("Sales");
                eb.HasKey(s => s.Id);

                eb.Property(s => s.SaleNumber)
                  .IsRequired();

                eb.Property(s => s.Date)
                  .IsRequired();

                eb.Property(s => s.IsCancelled)
                  .HasColumnName("IsCancelled")
                  .HasDefaultValue(false);

                eb.Property(s => s.Branch)
                  .HasColumnName("Branch")
                  .IsRequired();

                eb.OwnsOne(s => s.CustomerId, cb =>
                {
                    cb.Property(c => c.Value)
                      .HasColumnName("CustomerId")
                      .IsRequired();
                });               

                eb.OwnsMany(s => s.Items, ib =>
                {
                    ib.ToTable("SaleItems");
                    ib.HasKey(i => i.Id);

                    ib.Property(e => e.SaleId)
                      .IsRequired();

                    ib.Property(i => i.ProductId)
                      .IsRequired();

                    ib.Property(i => i.Quantity)
                      .IsRequired();

                    ib.Property(i => i.UnitPrice)
                      .HasColumnType("decimal(18,2)");

                    ib.Property(i => i.DiscountRate)
                      .HasColumnType("decimal(5,2)");

                    ib.Property<bool>("IsCancelled")
                      .HasColumnName("IsCancelled")
                      .HasDefaultValue(false);

                    ib.Property(i => i.Id)
                      .ValueGeneratedOnAdd();
                });
            });
            
            modelBuilder.Entity<Product>()
                .OwnsOne(
                    p => p.Rating,
                    r =>
                    {
                        r.Property(x => x.Rate)
                         .HasColumnName("RatingRate")
                         .IsRequired();

                        r.Property(x => x.Count)
                         .HasColumnName("RatingCount")
                         .IsRequired();
                    });
        }
    }

    /// <summary>
    /// Provides a design-time factory for creating instances of <see cref="DefaultContext"/>.
    /// Used by EF Core tools when performing migrations.
    /// </summary>
    public class YourDbContextFactory : IDesignTimeDbContextFactory<DefaultContext>
    {
        /// <summary>
        /// Creates a new <see cref="DefaultContext"/> instance using configuration settings from appsettings.json.
        /// </summary>
        /// <param name="args">Arguments passed by the design-time tools (ignored).</param>
        /// <returns>A configured <see cref="DefaultContext"/> instance.</returns>
        public DefaultContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<DefaultContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure Npgsql with the migrations assembly
            builder.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM"));

            // Enable detailed SQL logging (sensitive data included)
            builder.EnableSensitiveDataLogging();

            return new DefaultContext(builder.Options);
        }
    }
}
