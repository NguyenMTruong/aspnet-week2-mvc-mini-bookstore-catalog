namespace MiniBookstoreCatalog.Mvc.ViewModels;

public class AuditLogFilterViewModel
{
    public string? EntityName { get; set; }

    public string? Action { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public List<AuditLogViewModel> Logs { get; set; } = [];
}

