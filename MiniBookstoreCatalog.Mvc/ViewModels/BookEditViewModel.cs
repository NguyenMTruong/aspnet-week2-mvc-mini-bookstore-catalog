using System.ComponentModel.DataAnnotations;

namespace MiniBookstoreCatalog.Mvc.ViewModels;

public class BookEditViewModel
{
    public int Id { get; set; }

    [Required]
    public string ISBN { get; set; } = "";

    [Required]
    public string BookCode { get; set; } = "";

    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string Author { get; set; } = "";

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int MinStock { get; set; }

    public int CategoryId { get; set; }

    // Chuẩn bị cho Sprint 3.2
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

