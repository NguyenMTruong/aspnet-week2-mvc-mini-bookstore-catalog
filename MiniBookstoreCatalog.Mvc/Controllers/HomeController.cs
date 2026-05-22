using Microsoft.AspNetCore.Mvc;

namespace MiniBookstoreCatalog.Mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
