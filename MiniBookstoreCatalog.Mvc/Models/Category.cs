using MiniBookstoreCatalog.Mvc.Models.Base;

namespace MiniBookstoreCatalog.Mvc.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}

