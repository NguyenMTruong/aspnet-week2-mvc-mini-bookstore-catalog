using MiniBookstoreCatalog.Mvc.Models;

namespace MiniBookstoreCatalog.Mvc.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();

    Task<List<Book>> GetAllReadOnlyAsync();

    Task<Book?> GetByIdAsync(int id);

    Task<List<Book>> SearchAsync(
    string keyword,
    decimal? minPrice,
    string? stockStatus);

    Task AddAsync(Book book);

    Task UpdateAsync(Book book, byte[] rowVersion);

    Task DeleteAsync(int id);

    Task SaveChangesAsync();

    Task<List<Book>> FilterAsync(
    int? categoryId,
    decimal? minPrice,
    decimal? maxPrice);

    Task<List<Book>> GetDeletedAsync();

    Task RestoreAsync(int id);

    Task<bool> UpdateStockAsync(
    Book book,
    byte[] rowVersion);
}

