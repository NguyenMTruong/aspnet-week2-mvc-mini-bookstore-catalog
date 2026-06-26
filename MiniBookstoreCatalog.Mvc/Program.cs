using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Data;
using MiniBookstoreCatalog.Mvc.Services;
using MiniBookstoreCatalog.Mvc.Repositories;
using MiniBookstoreCatalog.Mvc.Options;

using Serilog;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"));
});

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/lab05-.txt", rollingInterval: RollingInterval.Day));

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("Application is running."), tags: new[] { "live" })
    .AddDbContextCheck<AppDbContext>("database", tags: new[] { "ready" });

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapGet("/api/books/{id:int}", async (int id, AppDbContext db, HttpContext http) =>
{
    var product = await db.Books.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    if (product == null)
    {
        return Results.Problem(
            type: "https://example.com/problems/product-not-found",
            title: "Product not found",
            detail: $"The product with id {id} was not found.",
            statusCode: StatusCodes.Status404NotFound,
            instance: http.Request.Path);
    }

    return Results.Ok(product);
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Home}/{action=Index}/{id?}");

app.Run();

