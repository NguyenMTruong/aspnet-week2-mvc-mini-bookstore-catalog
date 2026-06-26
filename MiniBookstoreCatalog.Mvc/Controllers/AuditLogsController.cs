using Microsoft.AspNetCore.Mvc;
using MiniBookstoreCatalog.Mvc.Services;
using MiniBookstoreCatalog.Mvc.ViewModels;

namespace MiniBookstoreCatalog.Mvc.Controllers;

public class AuditLogsController : Controller
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogsController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public async Task<IActionResult> Index(
        string? entityName,
        string? action,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var model = await _auditLogService.FilterAsync(
            entityName,
            action,
            fromDate,
            toDate);
        return View(model);
    }
}

