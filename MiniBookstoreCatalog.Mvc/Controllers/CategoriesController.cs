using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Data;

namespace MiniBookstoreCatalog.Mvc.Controllers;

public class CategoriesController
    : Controller
{
    private readonly AppDbContext _context;

    public CategoriesController(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories =
            await _context.Categories
                .Include(x => x.Books)
                .ToListAsync();

        return View(categories);
    }
}

