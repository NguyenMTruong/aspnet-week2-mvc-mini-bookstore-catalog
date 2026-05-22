using Microsoft.AspNetCore.Mvc;
using MiniBookstoreCatalog.Mvc.Models;
using MiniBookstoreCatalog.Mvc.Services;
using MiniBookstoreCatalog.Mvc.ViewModels;

namespace MiniBookstoreCatalog.Mvc.Controllers;

public class BooksController : Controller
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    public IActionResult Index()
    {
        var books = _bookService.GetAll()
            .Select(ToListItemViewModel)
            .ToList();

        return View(books);
    }

    public IActionResult Detail(int id)
    {
        var book = _bookService.GetById(id);

        if (book == null)
        {
            return NotFound($"Book with id = {id} not found");
        }

        return View(ToDetailViewModel(book));
    }

    public IActionResult Stats()
    {
        var stats = _bookService.GetStats();
        return View(stats);
    }

    public IActionResult Welcome()
    {
        return Content("Welcome to Mini Bookstore Catalog MVC");
    }

    public IActionResult BookJson()
    {
        var books = _bookService.GetAll();
        return Json(books);
    }

    public IActionResult GoToList()
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Force404()
    {
        return NotFound("404 Demo Response");
    }

    private static BookListItemViewModel ToListItemViewModel(Book book)
    {
        return new BookListItemViewModel
        {
            Id = book.Id,
            Code = book.Code,
            Title = book.Title,
            Category = book.Category,
            Price = book.Price,
            Quantity = book.Quantity,
            MinStock = book.MinStock
        };
    }

    private static BookDetailViewModel ToDetailViewModel(Book book)
    {
        return new BookDetailViewModel
        {
            Id = book.Id,
            Code = book.Code,
            Title = book.Title,
            Category = book.Category,
            Author = book.Author,
            Price = book.Price,
            Quantity = book.Quantity,
            MinStock = book.MinStock,
            LastUpdatedAt = book.LastUpdatedAt
        };
    }
}
