namespace MiniBookstoreCatalog.Mvc.ViewModels;

public class BookListItemViewModel
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public string Title { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int MinStock { get; set; }

    public string PriceText => $"{Price:N0} VND";

    public decimal InventoryValue => Price * Quantity;

    public string InventoryValueText => $"{InventoryValue:N0} VND";
    public string StockStatus
    {
        get
        {
            if (Quantity <= 0)
            {
                return "Hết hàng";
            }

            if (Quantity <= MinStock)
            {
                return "Cần nhập thêm";
            }

            return "Còn hàng";
        }
    }
    public string StockStatusClass
    {
        get
        {
            if (Quantity < 0)
            {
                return "badge badge-danger";
            }

            if (Quantity < MinStock)
            {
                return "badge badge-warning";
            }

            return "badge badge-success";
        }
    }
}
