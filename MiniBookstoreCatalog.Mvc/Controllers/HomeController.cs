using Microsoft.AspNetCore.Mvc;

namespace MiniBookstoreCatalog.Mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Error(int? id)
    {
        ViewBag.StatusCode = id;

        return View();
    }
}
