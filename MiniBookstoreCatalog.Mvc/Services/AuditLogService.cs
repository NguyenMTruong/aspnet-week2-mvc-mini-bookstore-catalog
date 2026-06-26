using MiniBookstoreCatalog.Mvc.Repositories;
using MiniBookstoreCatalog.Mvc.ViewModels;
using Microsoft.Extensions.Options;
using MiniBookstoreCatalog.Mvc.Options;

namespace MiniBookstoreCatalog.Mvc.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _repository;
    private readonly AppSettings _settings;

    public AuditLogService(
        IAuditLogRepository repository,
        IOptions<AppSettings> options)
    {
        _repository = repository;
        _settings = options.Value;
    }

    public async Task<List<AuditLogViewModel>> GetAllAsync()
    {
        var logs = await _repository.GetAllAsync();

        return logs.Select(x => new AuditLogViewModel
        {
            Id = x.Id,
            EntityName = x.EntityName,
            EntityId = x.EntityId,
            Action = x.Action,
            CreatedAt = x.CreatedAt,
            UserName = x.UserName,
            OldValues = x.OldValues,
            NewValues = x.NewValues
        }).ToList();
    }

    public async Task<AuditLogFilterViewModel> FilterAsync(
    string? entityName,
    string? action,
    DateTime? fromDate,
    DateTime? toDate)
    {
        var logs = await _repository.FilterAsync(
            entityName,
            action,
            fromDate,
            toDate);

        return new AuditLogFilterViewModel
        {
            EntityName = entityName,
            Action = action,
            FromDate = fromDate,
            ToDate = toDate,
            Logs = logs.Select(x => new AuditLogViewModel
            {
                Id = x.Id,
                EntityName = x.EntityName,
                EntityId = x.EntityId,
                Action = x.Action,
                CreatedAt = x.CreatedAt,
                UserName = x.UserName,
                OldValues = x.OldValues,
                NewValues = x.NewValues
            }).ToList()
        };
    }
}

