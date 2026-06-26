using MiniBookstoreCatalog.Mvc.Models;

namespace MiniBookstoreCatalog.Mvc.Repositories;

public interface IAuditLogRepository
{
    Task<List<AuditLog>> GetAllAsync();

    Task<List<AuditLog>> FilterAsync(
    string? entityName,
    string? action,
    DateTime? fromDate,
    DateTime? toDate);
}

