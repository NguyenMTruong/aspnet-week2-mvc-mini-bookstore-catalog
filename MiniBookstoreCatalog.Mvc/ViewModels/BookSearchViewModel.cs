namespace MiniBookstoreCatalog.Mvc.ViewModels;

public class BookSearchViewModel
{
    public string Keyword { get; set; } = "";

    public decimal? MinPrice { get; set; }

    public List<BookListItemViewModel> Books { get; set; } = new();
}
