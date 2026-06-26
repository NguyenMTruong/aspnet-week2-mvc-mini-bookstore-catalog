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
    private readonly ILogger<BookService> _logger;

    public BookService(
        IBookRepository repository,
        IOptions<AppSettings> options,
        ILogger<BookService> logger)
    {
        _repository = repository;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<List<BookListItemViewModel>> GetAllAsync()
    {
        var books =
            await _repository.GetAllReadOnlyAsync();

        return books.Select(ToListItem)
            .ToList();
    }

    public async Task<BookDetailViewModel?> GetByIdAsync(int id)
    {
        var book =
            await _repository.GetByIdAsync(id);

        if (book == null)
            return null;

        return ToDetail(book);
    }

    public async Task<List<BookListItemViewModel>> SearchAsync(
    string keyword,
    decimal? minPrice,
    string? stockStatus)
    {

        var books =
            await _repository.SearchAsync(
                keyword,
                minPrice,
                stockStatus);



        return books
            .Select(ToListItem)
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

    public async Task<BookStatsViewModel> GetStatsAsync()
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

    private static BookListItemViewModel ToListItem(Book book)
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

    private static BookDetailViewModel ToDetail(Book book)
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

    public async Task<List<BookListItemViewModel>> GetDeletedAsync()
    {
        var books =
            await _repository.GetDeletedAsync();

        return books
            .Select(ToListItem)
            .ToList();
    }

    public async Task<BookEditViewModel?> GetForEditAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book == null)
            return null;

        return new BookEditViewModel
        {
            Id = book.Id,
            ISBN = book.ISBN,
            BookCode = book.BookCode,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            Stock = book.Stock,
            MinStock = book.MinStock,
            CategoryId = book.CategoryId,

            RowVersion = book.RowVersion
        };
    }

    public async Task UpdateAsync(BookEditViewModel model)
    {
        var book = await _repository.GetByIdAsync(model.Id);

        if (book == null)
            throw new Exception("Book not found.");

        book.ISBN = model.ISBN;
        book.BookCode = model.BookCode;
        book.Title = model.Title;
        book.Author = model.Author;
        book.Price = model.Price;
        book.Stock = model.Stock;
        book.MinStock = model.MinStock;
        book.CategoryId = model.CategoryId;

        // Tạm thời vẫn giữ để không ảnh hưởng các View hiện có
        book.LastUpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(book, model.RowVersion);

        await _repository.SaveChangesAsync();
    }

    public async Task<AdjustStockViewModel?> GetAdjustStockAsync(int id)
    {

        var book =
        await _repository.GetByIdAsync(id);



        if (book == null)
            return null;



        return new AdjustStockViewModel
        {

            Id = book.Id,

            RowVersion = book.RowVersion

        };

    }

    public async Task<bool> AdjustStockAsync(
AdjustStockViewModel model)
    {


        var book =
        await _repository.GetByIdAsync(model.Id);



        if (book == null)
            return false;



        var newStock =
        book.Stock + model.ChangeAmount;



        if (newStock < 0)
            return false;



        book.Stock = newStock;


        book.LastUpdatedAt =
        DateTime.Now;



        _logger.LogInformation(
        "Adjust stock BookId={id} Stock={stock}",
        book.Id,
        book.Stock);



        return await _repository
        .UpdateStockAsync(
        book,
        model.RowVersion);

    }
}

