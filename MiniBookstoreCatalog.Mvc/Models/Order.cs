using MiniBookstoreCatalog.Mvc.Models.Base;

namespace MiniBookstoreCatalog.Mvc.Models;

public class Order : BaseEntity
{
    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

