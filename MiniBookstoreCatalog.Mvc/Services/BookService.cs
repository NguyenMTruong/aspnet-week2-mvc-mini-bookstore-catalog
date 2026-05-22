using MiniBookstoreCatalog.Mvc.Models;
using MiniBookstoreCatalog.Mvc.ViewModels;

namespace MiniBookstoreCatalog.Mvc.Services;

public class BookService
{
    private readonly List<Book> _books = new()
    {
        new Book
        {
            Id = 1,
            Code = "BK-001",
            Title = "Clean Code",
            Category = "Programming",
            Author = "Robert C. Martin",
            Price = 450000,
            Quantity = 12,
            MinStock = 5,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 2,
            Code = "BK-002",
            Title = "ASP.NET Core MVC",
            Category = "Technology",
            Author = "Microsoft Press",
            Price = 520000,
            Quantity = 3,
            MinStock = 5,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 3,
            Code = "BK-003",
            Title = "Design Patterns",
            Category = "Programming",
            Author = "GoF",
            Price = 600000,
            Quantity = 0,
            MinStock = 2,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 4,
            Code = "BK-004",
            Title = "The Pragmatic Programmer",
            Category = "Programming",
            Author = "Andrew Hunt",
            Price = 480000,
            Quantity = 7,
            MinStock = 3,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 5,
            Code = "BK-005",
            Title = "Refactoring",
            Category = "Software Engineering",
            Author = "Martin Fowler",
            Price = 550000,
            Quantity = 2,
            MinStock = 4,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 6,
            Code = "BK-006",
            Title = "Head First Java",
            Category = "Programming",
            Author = "Kathy Sierra",
            Price = 390000,
            Quantity = 10,
            MinStock = 5,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 7,
            Code = "BK-007",
            Title = "Entity Framework Core",
            Category = "Technology",
            Author = "Julie Lerman",
            Price = 470000,
            Quantity = 1,
            MinStock = 3,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 8,
            Code = "BK-008",
            Title = "C# in Depth",
            Category = "Programming",
            Author = "Jon Skeet",
            Price = 650000,
            Quantity = 8,
            MinStock = 4,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 9,
            Code = "BK-009",
            Title = "Introduction to Algorithms",
            Category = "Computer Science",
            Author = "Thomas H. Cormen",
            Price = 720000,
            Quantity = 5,
            MinStock = 2,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 10,
            Code = "BK-010",
            Title = "Docker Deep Dive",
            Category = "DevOps",
            Author = "Nigel Poulton",
            Price = 430000,
            Quantity = 0,
            MinStock = 2,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 11,
            Code = "BK-011",
            Title = "Kubernetes Up & Running",
            Category = "Cloud Computing",
            Author = "Kelsey Hightower",
            Price = 580000,
            Quantity = 6,
            MinStock = 3,
            LastUpdatedAt = DateTime.Now
        },

        new Book
        {
            Id = 12,
            Code = "BK-012",
            Title = "JavaScript: The Good Parts",
            Category = "Web Development",
            Author = "Douglas Crockford",
            Price = 310000,
            Quantity = 9,
            MinStock = 4,
            LastUpdatedAt = DateTime.Now
        }
    };

    public List<Book> GetAll()
    {
        return _books;
    }

    public Book? GetById(int id)
    {
        return _books.FirstOrDefault(book => book.Id == id);
    }

    public BookStatsViewModel GetStats()
    {
        return new BookStatsViewModel
        {
            TotalBooks = _books.Count,
            TotalQuantity = _books.Sum(book => book.Quantity),
            TotalInventoryValue = _books.Sum(book => book.Price * book.Quantity),

            OutOfStockCount = _books.Count(book => book.Quantity <= 0),

            NeedReorderCount = _books.Count(book =>
                book.Quantity > 0 &&
                book.Quantity <= book.MinStock)
        };
    }
}
