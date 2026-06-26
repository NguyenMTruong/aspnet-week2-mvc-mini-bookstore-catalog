using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Data;
using MiniBookstoreCatalog.Mvc.Models;

namespace MiniBookstoreCatalog.Mvc.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<Book>> GetAllReadOnlyAsync()
    {
        return await _context.Books
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                !x.IsDeleted);
    }

    public async Task<List<Book>> SearchAsync(
    string keyword,
    decimal? minPrice,
    string? stockStatus)
    {

        var query =
            _context.Books
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted);



        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x =>
                x.Title.Contains(keyword)
                ||
                x.Author.Contains(keyword)
                ||
                x.ISBN.Contains(keyword));
        }



        if (minPrice.HasValue)
        {
            query = query.Where(x =>
                x.Price >= minPrice.Value);
        }



        if (stockStatus == "low")
        {
            query = query.Where(x =>
                x.Stock < x.MinStock);
        }



        return await query.ToListAsync();

    }
    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public Task UpdateAsync(Book book, byte[] rowVersion)
    {
        _context.Entry(book)
            .Property(x => x.RowVersion)
            .OriginalValue = rowVersion;
        _context.Books.Update(book);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(x => x.Id == id);

        if (book != null)
        {
            book.IsDeleted = true;
            book.DeletedAt = DateTime.UtcNow;
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Book>> FilterAsync(
    int? categoryId,
    decimal? minPrice,
    decimal? maxPrice)
    {
        var query = _context.Books
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(x =>
                x.CategoryId == categoryId);

        if (minPrice.HasValue)
            query = query.Where(x =>
                x.Price >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(x =>
                x.Price <= maxPrice);

        return await query.ToListAsync();
    }

    public async Task<List<Book>> GetDeletedAsync()
    {
        return await _context.Books
            .Include(x => x.Category)
            .Where(x => x.IsDeleted)
            .ToListAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(x => x.Id == id);

        if (book == null)
            return;

        book.IsDeleted = false;
        book.DeletedAt = null;
    }
}

