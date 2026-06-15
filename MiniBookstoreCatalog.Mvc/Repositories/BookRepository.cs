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
            .ToListAsync();
    }

    public async Task<List<Book>> GetAllReadOnlyAsync()
    {
        return await _context.Books
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Book>> SearchAsync(string keyword)
    {
        keyword = keyword.ToLower();

        return await _context.Books
            .Include(x => x.Category)
            .Where(x =>
                x.Title.ToLower().Contains(keyword)
                || x.Author.ToLower().Contains(keyword)
                || x.ISBN.ToLower().Contains(keyword))
            .ToListAsync();
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(x => x.Id == id);

        if (book != null)
        {
            _context.Books.Remove(book);
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
}

