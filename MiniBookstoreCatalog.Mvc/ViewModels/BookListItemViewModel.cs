namespace MiniBookstoreCatalog.Mvc.ViewModels;

public class BookListItemViewModel
{
    public int Id { get; set; }
    public string ISBN { get; set; } = "";
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string CategoryName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int MinStock { get; set; }

    public string PriceText => $"{Price:N0} VND";

    public decimal InventoryValue => Price * Stock;

    public string InventoryValueText => $"{InventoryValue:N0} VND";
    public string StockStatus
    {
        get
        {
            if (Stock <= 0)
            {
                return "Hết hàng";
            }

            if (Stock <= MinStock)
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
            if (Stock < 0)
            {
                return "badge badge-danger";
            }

            if (Stock < MinStock)
            {
                return "badge badge-warning";
            }

            return "badge badge-success";
        }
    }
}
