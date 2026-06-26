using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MiniBookstoreCatalog.Mvc.Models.Base;

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

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
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

        // NEW
        modelBuilder.Entity<Book>()
            .Property(x => x.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Category>()
            .Property(x => x.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Order>()
            .Property(x => x.RowVersion)
            .IsRowVersion();

        SeedData(modelBuilder);
    }
    private static void SeedData(ModelBuilder modelBuilder)
    {
        var createdDate = new DateTime(2025, 1, 1);

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Programming",
                CreatedAt = createdDate,
                UpdatedAt = null,
                IsDeleted = false,
                DeletedAt = null
            },
            new Category
            {
                Id = 2,
                Name = "Database",
                CreatedAt = createdDate,
                UpdatedAt = null,
                IsDeleted = false,
                DeletedAt = null
            },
            new Category
            {
                Id = 3,
                Name = "Web Development",
                CreatedAt = createdDate,
                UpdatedAt = null,
                IsDeleted = false,
                DeletedAt = null
            }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                BookCode = "BK001",
                ISBN = "9780132350884",
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Price = 350000,
                Stock = 15,
                MinStock = 5,
                CategoryId = 1,

                // Audit Fields
                CreatedAt = createdDate,
                UpdatedAt = null,
                IsDeleted = false,
                DeletedAt = null,

                // Tạm giữ đến Sprint 3
                LastUpdatedAt = createdDate
            },
            new Book
            {
                Id = 2,
                BookCode = "BK002",
                ISBN = "9781617294532",
                Title = "ASP.NET Core In Action",
                Author = "Andrew Lock",
                Price = 550000,
                Stock = 10,
                MinStock = 3,
                CategoryId = 3,

                CreatedAt = createdDate,
                UpdatedAt = null,
                IsDeleted = false,
                DeletedAt = null,

                LastUpdatedAt = createdDate
            },
            new Book
            {
                Id = 3,
                BookCode = "BK003",
                ISBN = "9781492057611",
                Title = "Learning SQL",
                Author = "Alan Beaulieu",
                Price = 420000,
                Stock = 8,
                MinStock = 3,
                CategoryId = 2,

                CreatedAt = createdDate,
                UpdatedAt = null,
                IsDeleted = false,
                DeletedAt = null,

                LastUpdatedAt = createdDate
            }
        );
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>();

        foreach (EntityEntry<BaseEntity> entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();

        return await base.SaveChangesAsync(cancellationToken);
    }
}

