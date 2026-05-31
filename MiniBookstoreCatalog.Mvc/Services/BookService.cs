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

    public List<Book> Search(string? keyword, decimal? minPrice)
    {
        var query = _books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(book =>
                book.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                book.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                book.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(product => product.Price >= minPrice.Value);
        }

        return query.ToList();
    }

    public Book Create(BookCreateViewModel model)
    {
        var newId = _books.Count == 0
            ? 1
            : _books.Max(product => product.Id) + 1;

        var book = new Book
        {
            Id = newId,
            Code = $"NEW-{newId:000}",
            Title = model.Title,
            Category = model.Category,
            Author = model.Author,
            Price = model.Price,
            Quantity = model.Quantity,
            MinStock = model.MinStock,
            LastUpdatedAt = DateTime.Now
        };

        _books.Add(book);

        return book;
    }

}
