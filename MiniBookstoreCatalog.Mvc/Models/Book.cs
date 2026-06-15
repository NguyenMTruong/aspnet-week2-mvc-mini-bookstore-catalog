namespace MiniBookstoreCatalog.Mvc.Models;

using System.ComponentModel.DataAnnotations;

public class Book
{
    public int Id { get; set; }

    public string ISBN { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string BookCode { get; set; } = "";

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int MinStock { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public int CategoryId { get; set; }

    public Category ? Category { get; set; }
}

