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
        var query = _context.AuditLogs
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(entityName))
        {
            entityName = entityName.Trim().ToLower();

            query = query.Where(x =>
                x.EntityName != null &&
                x.EntityName.ToLower().Contains(entityName));
        }

        // if (!string.IsNullOrWhiteSpace(action))
        // {
        //     query = query.Where(x =>
        //         x.Action != null &&
        //         x.Action.ToUpper() == action.ToUpper());
        // }
        if (fromDate.HasValue)
        {
            Console.WriteLine($"fromDate: {fromDate}");
            var from = fromDate.Value.Date;
            query = query.Where(x => x.CreatedAt >= from);
        }
        if (toDate.HasValue)
        {
            Console.WriteLine($"toDate: {toDate}");
            var to = toDate.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(x => x.CreatedAt <= to);
        }
        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}

