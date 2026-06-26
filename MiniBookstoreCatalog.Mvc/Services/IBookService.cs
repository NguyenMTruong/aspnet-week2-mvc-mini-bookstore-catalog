using MiniBookstoreCatalog.Mvc.ViewModels;
using MiniBookstoreCatalog.Mvc.Models;

namespace MiniBookstoreCatalog.Mvc.Services;

public interface IBookService
{
    Task<List<BookListItemViewModel>> GetAllAsync();

    Task<BookDetailViewModel?> GetByIdAsync(int id);

    Task<BookStatsViewModel> GetStatsAsync();

    Task<List<BookListItemViewModel>> SearchAsync(
    string keyword,
    decimal? minPrice,
    string? stockStatus);

    Task CreateAsync(BookCreateViewModel model);

    Task<List<Book>> GetLowStockBooksAsync();

    Task<List<BookListItemViewModel>> FilterAsync(
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice);

    Task DeleteAsync(int id);

    Task<List<BookListItemViewModel>> GetDeletedAsync();

    Task RestoreAsync(int id);

    Task<BookEditViewModel?> GetForEditAsync(int id);

    Task UpdateAsync(BookEditViewModel model);

    Task<AdjustStockViewModel?> GetAdjustStockAsync(int id);

    Task<bool> AdjustStockAsync(AdjustStockViewModel model);
}

