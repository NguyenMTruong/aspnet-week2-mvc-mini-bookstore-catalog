using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Services;
using MiniBookstoreCatalog.Mvc.ViewModels;

namespace MiniBookstoreCatalog.Mvc.Controllers;

public class BooksController : Controller
{
    private readonly IBookService _bookService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(
    IBookService bookService,
    ILogger<BooksController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation(
            "Loading book list");

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
            return Problem(
                statusCode: 404,
                title: "Book not found",
                Extensions: new Dictionary<string, object?>
                {
                     {
                    "errorCode",
                    "BOOK_NOT_FOUND"
                },

                {
                    "traceId",
                    HttpContext.TraceIdentifier
                }
                }
            );
        }

        return View(book);
    }

    public async Task<IActionResult> Stats()
    {
        var stats =
            await _bookService.GetStatsAsync();

        return View(stats);
    }

    // HTTP GET
    [HttpGet]
    public IActionResult Create()
    {
        return View(
            new BookCreateViewModel());
    }

    public async Task<IActionResult> AdjustStock(int id)
    {

        var model =
        await _bookService
        .GetAdjustStockAsync(id);



        if (model == null)
            return Problem(
                statusCode: 404,
                title: "Book not found",
                Extensions: new Dictionary<string, object?>
                {
                     {
                    "errorCode",
                    "BOOK_NOT_FOUND"
                },

                {
                    "traceId",
                    HttpContext.TraceIdentifier
                }
                }
            );



        return View(model);

    }

    private IActionResult Problem(int statusCode, string title, Dictionary<string, object?> Extensions)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> Search(
    string keyword,
    decimal? minPrice,
    string? stockStatus)
    {

        var result =
            await _bookService.SearchAsync(
                keyword,
                minPrice,
                stockStatus);



        var model = new BookSearchViewModel
        {
            Keyword = keyword,

            MinPrice = minPrice,

            StockStatus = stockStatus,

            Books = result
        };



        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _bookService.GetForEditAsync(id);

        if (model == null)
            return Problem(
                statusCode: 404,
                title: "Book not found",
                Extensions: new Dictionary<string, object?>
                {
                     {
                    "errorCode",
                    "BOOK_NOT_FOUND"
                },

                {
                    "traceId",
                    HttpContext.TraceIdentifier
                }
                }
            );

        return View(model);
    }

    // HTTP POST
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

    [HttpPost]
    public async Task<IActionResult> AdjustStock(
    AdjustStockViewModel model)
    {


        var result =
        await _bookService
        .AdjustStockAsync(model);



        if (!result)
        {

            ModelState.AddModelError(
            "",
            "Stock không hợp lệ hoặc dữ liệu đã thay đổi");


            return View(model);

        }



        return RedirectToAction("Index");

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _bookService.DeleteAsync(id);

        TempData["Success"] =
            "Xóa sách thành công.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        await _bookService.RestoreAsync(id);

        TempData["Success"] =
            "Khôi phục thành công.";

        return RedirectToAction(nameof(Trash));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    BookEditViewModel model)
    {

        if (!ModelState.IsValid)
            return View(model);



        try
        {

            await _bookService
                .UpdateAsync(model);



            TempData["Success"] =
                "Cập nhật thành công.";


            return RedirectToAction(
                nameof(Index));

        }
        catch (DbUpdateConcurrencyException)
        {

            ModelState.AddModelError(
                "",
                "Dữ liệu đã bị thay đổi bởi người khác. Vui lòng tải lại.");


            return View(model);
        }

    }

    // METHOD
    public async Task<IActionResult> LowStock()
    {
        var books =
            await _bookService.GetLowStockBooksAsync();

        return View(books);
    }

    public async Task<IActionResult> Filter(
    int? categoryId,
    decimal? minPrice,
    decimal? maxPrice)
    {
        var result =
            await _bookService.FilterAsync(
                categoryId,
                minPrice,
                maxPrice);

        return View(result);
    }

    public async Task<IActionResult> Trash()
    {
        var books =
            await _bookService.GetDeletedAsync();

        return View(books);
    }
}

