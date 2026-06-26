using MiniBookstoreCatalog.Mvc.ViewModels;

namespace MiniBookstoreCatalog.Mvc.Services;

public interface IAuditLogService
{
    Task<List<AuditLogViewModel>> GetAllAsync();

    Task<AuditLogFilterViewModel> FilterAsync(
    string? entityName,
    string? action,
    DateTime? fromDate,
    DateTime? toDate);
}

