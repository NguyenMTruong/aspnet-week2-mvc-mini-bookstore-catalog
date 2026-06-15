using Microsoft.AspNetCore.Mvc;
using MiniBookstoreCatalog.Mvc.Services;
using MiniBookstoreCatalog.Mvc.ViewModels;

namespace MiniBookstoreCatalog.Mvc.Controllers;

public class BooksController : Controller
{
    private readonly IBookService _bookService;

    public BooksController(
        IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<IActionResult> Index()
    {
        var books =
            await _bookService.GetAllAsync();

        return View(books);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var book =
            await _bookService.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    public async Task<IActionResult> Stats()
    {
        var stats =
            await _bookService.GetStatsAsync();

        return View(stats);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(
            new BookCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        BookCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _bookService.CreateAsync(model);

        TempData["Success"] =
            "Thêm sách thành công";

        return RedirectToAction(
            nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        string keyword)
    {
        if (string.IsNullOrWhiteSpace(
            keyword))
        {
            var books =
                await _bookService
                    .GetAllAsync();

            return View(books);
        }

        var result =
            await _bookService
                .SearchAsync(keyword);

        return View(result);
    }
}

