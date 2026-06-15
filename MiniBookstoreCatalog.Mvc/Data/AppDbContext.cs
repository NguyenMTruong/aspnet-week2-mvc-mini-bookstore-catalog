using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Models;

namespace MiniBookstoreCatalog.Mvc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Books)
            .WithOne(b => b.Category)
            .HasForeignKey(b => b.CategoryId);

        modelBuilder.Entity<Book>()
            .Property(b => b.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Programming"
            },
            new Category
            {
                Id = 2,
                Name = "Database"
            },
            new Category
            {
                Id = 3,
                Name = "Web Development"
            }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                ISBN = "9780132350884",
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Price = 350000,
                Stock = 15,
                MinStock = 5,
                CategoryId = 1,
                LastUpdatedAt = new DateTime(2025, 1, 1)
            },
            new Book
            {
                Id = 2,
                ISBN = "9781617294532",
                Title = "ASP.NET Core In Action",
                Author = "Andrew Lock",
                Price = 550000,
                Stock = 10,
                MinStock = 3,
                CategoryId = 3,
                LastUpdatedAt = new DateTime(2025, 1, 1)
            },
            new Book
            {
                Id = 3,
                ISBN = "9781492057611",
                Title = "Learning SQL",
                Author = "Alan Beaulieu",
                Price = 420000,
                Stock = 8,
                MinStock = 3,
                CategoryId = 2,
                LastUpdatedAt = new DateTime(2025, 1, 1)
            }
        );
    }
}

