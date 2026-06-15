using MiniBookstoreCatalog.Mvc.ViewModels;

namespace MiniBookstoreCatalog.Mvc.Services;

public interface IBookService
{
    Task<List<BookListItemViewModel>> GetAllAsync();

    Task<BookDetailViewModel?> GetByIdAsync(int id);

    Task<BookStatsViewModel> GetStatsAsync();

    Task<List<BookListItemViewModel>> SearchAsync(string keyword);

    Task CreateAsync(BookCreateViewModel model);
}

