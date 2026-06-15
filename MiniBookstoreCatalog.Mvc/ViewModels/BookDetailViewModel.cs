namespace MiniBookstoreCatalog.Mvc.ViewModels;

public class BookDetailViewModel
{
    public int Id { get; set; }
    public string ISBN { get; set; } = "";
    public string Title { get; set; } = "";
    public string CategoryName { get; set; } = string.Empty;
    public string Author { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int MinStock { get; set; }
    public DateTime LastUpdatedAt { get; set; }

    public string PriceText => $"{Price:N0} VND";

    public decimal InventoryValue => Price * Stock;

    public string InventoryValueText => $"{InventoryValue:N0} VND";

    public string LastUpdatedText => LastUpdatedAt.ToString("dd/MM/yyyy HH:mm");

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

    public string ReorderSuggestion
    {
        get
        {
            if (Stock < 0)
            {
                return "Cần nhập hàng ngay vì sản phẩm đã hết.";
            }

            if (Stock < MinStock)
            {
                return $"Nên nhập thêm. Tồn kho hiện tại chỉ còn {Stock}, mức tối thiểu là {MinStock}";
            }
            return "Tồn kho đang ổn định, chưa cần nhập thêm.";
        }
    }
}
