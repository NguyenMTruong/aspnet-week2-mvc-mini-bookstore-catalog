using MiniBookstoreCatalog.Mvc.ViewModels;

namespace MiniBookstoreCatalog.Mvc.Services;

public interface IOrderService
{
    Task CreateOrderAsync(
        OrderCreateViewModel model);
}

