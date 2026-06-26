using Microsoft.AspNetCore.Mvc;
using MiniBookstoreCatalog.Mvc.Services;
using MiniBookstoreCatalog.Mvc.ViewModels;

namespace MiniBookstoreCatalog.Mvc.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderService _orderService;

    public OrdersController(
        IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(
            new OrderCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        OrderCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _orderService
                .CreateOrderAsync(model);

            return RedirectToAction(
                nameof(Success));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                "",
                ex.Message);

            return View(model);
        }
    }

    public IActionResult Success()
    {
        return View();
    }
}

