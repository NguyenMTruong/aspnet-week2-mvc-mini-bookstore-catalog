using MiniBookstoreCatalog.Mvc.Models.Base;

namespace MiniBookstoreCatalog.Mvc.Models;

public class AuditLog : BaseEntity
{
    public string EntityName { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? UserName { get; set; }
}

