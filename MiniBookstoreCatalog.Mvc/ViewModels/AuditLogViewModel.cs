namespace MiniBookstoreCatalog.Mvc.ViewModels;

public class AuditLogViewModel
{
    public int Id { get; set; }

    public string EntityName { get; set; } = "";

    public int EntityId { get; set; }

    public string Action { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public string? UserName { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }
}

