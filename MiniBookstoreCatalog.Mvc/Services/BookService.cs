using MiniBookstoreCatalog.Mvc.Models;
using MiniBookstoreCatalog.Mvc.Repositories;
using MiniBookstoreCatalog.Mvc.ViewModels;
using Microsoft.Extensions.Options;
using MiniBookstoreCatalog.Mvc.Options;

namespace MiniBookstoreCatalog.Mvc.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    private readonly AppSettings _settings;

    public BookService(
        IBookRepository repository,
        IOptions<AppSettings> options)
    {
        _repository = repository;
        _settings = options.Value;
    }

    public async Task<List<BookListItemViewModel>>
        GetAllAsync()
    {
        var books =
            await _repository.GetAllReadOnlyAsync();

        return books.Select(ToListItem)
            .ToList();
    }

    public async Task<BookDetailViewModel?>
        GetByIdAsync(int id)
    {
        var book =
            await _repository.GetByIdAsync(id);

        if (book == null)
            return null;

        return ToDetail(book);
    }

    public async Task<List<BookListItemViewModel>>
        SearchAsync(string keyword)
    {
        var books =
            await _repository.SearchAsync(keyword);

        return books.Select(ToListItem)
            .ToList();
    }

    public async Task CreateAsync(
        BookCreateViewModel model)
    {
        var book = new Book
        {
            ISBN = model.ISBN,
            Title = model.Title,
            Author = model.Author,
            Price = model.Price,
            Stock = model.Stock,
            MinStock = model.MinStock,
            CategoryId = model.CategoryId,
            LastUpdatedAt = DateTime.Now
        };

        await _repository.AddAsync(book);

        await _repository.SaveChangesAsync();
    }

    public async Task<BookStatsViewModel>
        GetStatsAsync()
    {
        var books =
            await _repository.GetAllReadOnlyAsync();

        return new BookStatsViewModel
        {
            TotalBooks = books.Count,
            TotalQuantity =
                books.Sum(x => x.Stock),

            TotalInventoryValue =
                books.Sum(x =>
                    x.Price * x.Stock),

            OutOfStockCount =
                books.Count(x =>
                    x.Stock == 0),

            NeedReorderCount =
                books.Count(x =>
                    x.Stock > 0 &&
                    x.Stock <= x.MinStock)
        };
    }

    private static BookListItemViewModel
        ToListItem(Book book)
    {
        return new BookListItemViewModel
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            Stock = book.Stock,
            MinStock = book.MinStock,
            CategoryName =
                book.Category?.Name ?? ""
        };
    }

    private static BookDetailViewModel
        ToDetail(Book book)
    {
        return new BookDetailViewModel
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            Stock = book.Stock,
            MinStock = book.MinStock,
            LastUpdatedAt =
                book.LastUpdatedAt,
            CategoryName =
                book.Category?.Name ?? ""
        };
    }

    public async Task<List<Book>> GetLowStockBooksAsync()
    {
        var books = await _repository.GetAllAsync();

        return books
            .Where(x => x.Stock <= _settings.LowAvailableCopyThreshold)
            .ToList();
    }

    public async Task<List<BookListItemViewModel>> FilterAsync(
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice)
    {
        var books =
            await _repository.FilterAsync(
                categoryId,
                minPrice,
                maxPrice);

        return books.Select(x =>
            new BookListItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price
            })
            .ToList();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        await _repository.RestoreAsync(id);

        await _repository.SaveChangesAsync();
    }

    public async Task<List<BookListItemViewModel>>
    GetDeletedAsync()
    {
        var books =
            await _repository.GetDeletedAsync();

        return books
            .Select(ToListItem)
            .ToList();
    }


}

