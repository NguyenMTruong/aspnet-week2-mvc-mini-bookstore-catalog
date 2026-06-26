using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Data;
using MiniBookstoreCatalog.Mvc.Models;

namespace MiniBookstoreCatalog.Mvc.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await _context.AuditLogs
            .OrderByDescending(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<AuditLog>> FilterAsync(
    string? entityName,
    string? action,
    DateTime? fromDate,
    DateTime? toDate)
    {
        var query = _context.AuditLogs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(entityName))
            query = query.Where(x => x.EntityName.Contains(entityName));

        if (!string.IsNullOrWhiteSpace(action))
            query = query.Where(x => x.Action == action);

        if (fromDate.HasValue)
            query = query.Where(x => x.CreatedAt >= fromDate);

        if (toDate.HasValue)
            query = query.Where(x => x.CreatedAt <= toDate);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }
}

