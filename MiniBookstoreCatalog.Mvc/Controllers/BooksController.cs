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

    [HttpGet]
    public IActionResult Search(string? keyword, decimal? minPrice)
    {
        var products = _bookService.Search(keyword, minPrice)
            .Select(ToListItemViewModel)
            .ToList();

        var viewModel = new BookSearchViewModel
        {
            Keyword = keyword ?? "",
            MinPrice = minPrice,
            Books = products
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new BookCreateViewModel
        {
            Quantity = 1,
            MinStock = 1
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BookCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _bookService.Create(model);

        TempData["SuccessMessage"] = "Đã thêm sản phẩm thành công.";

        return RedirectToAction(nameof(Index));
    }

}
