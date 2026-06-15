using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Data;

namespace MiniBookstoreCatalog.Mvc.Controllers;

public class DataHealthController
    : Controller
{
    private readonly AppDbContext _context;

    public DataHealthController(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var canConnect =
            await _context.Database
                .CanConnectAsync();

        ViewBag.DatabaseStatus =
            canConnect
                ? "Connected"
                : "Disconnected";

        return View();
    }
}

