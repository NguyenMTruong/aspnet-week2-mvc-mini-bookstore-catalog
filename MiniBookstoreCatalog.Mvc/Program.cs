using Microsoft.EntityFrameworkCore;
using MiniBookstoreCatalog.Mvc.Data;
using MiniBookstoreCatalog.Mvc.Services;
using MiniBookstoreCatalog.Mvc.Repositories;
using MiniBookstoreCatalog.Mvc.Options;
using Serilog;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);


// Serilog
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(
            "logs/lab05-.txt",
            rollingInterval: RollingInterval.Day));



// Options
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));



// MVC
builder.Services.AddControllersWithViews();


// Feature 3:
// ProblemDetails
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["traceId"] =
            context.HttpContext.TraceIdentifier;


        context.ProblemDetails.Extensions["timestamp"] =
            DateTimeOffset.UtcNow;


        // không trả stack trace/detail
        context.ProblemDetails.Detail = null;
    };
});



// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration
        .GetConnectionString("DefaultConnection"));
});



// Dependency Injection

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();



// Health Check
builder.Services.AddHealthChecks()

    .AddCheck(
        "self",
        () => HealthCheckResult.Healthy(
            "Application is running."),
        tags: new[] { "live" })


    .AddDbContextCheck<AppDbContext>(
        "database",
        tags: new[] { "ready" });



var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Feature 3
    app.UseExceptionHandler();

    app.UseHsts();
}



app.UseStatusCodePagesWithReExecute(
    "/Home/StatusCode",
    "?code={0}");



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();




// Health live
app.MapHealthChecks(
    "/health/live",
    new HealthCheckOptions
    {
        Predicate =
            check => check.Tags.Contains("live")
    });




// Health ready custom JSON
app.MapHealthChecks(
    "/health/ready",
    new HealthCheckOptions
    {

        Predicate =
            check => check.Tags.Contains("ready"),


        ResponseWriter = async (context, report) =>
        {

            context.Response.ContentType =
                "application/json";


            await context.Response.WriteAsJsonAsync(new
            {

                status =
                    report.Status.ToString(),


                checks =
                    report.Entries.Select(x => new
                    {

                        name = x.Key,

                        status =
                            x.Value.Status.ToString(),

                        description =
                            x.Value.Description

                    })

            });

        }
    });




// API test ProblemDetails
app.MapGet(
"/api/products/{id:int}",
async (
    int id,
    AppDbContext db,
    HttpContext http) =>
{

    var product =
        await db.Books
        .AsNoTracking()
        .FirstOrDefaultAsync(
            p => p.Id == id);



    if(product == null)
    {

        return Results.Problem(

            title:
                "Product not found",


            statusCode:
                StatusCodes.Status404NotFound,


            extensions:
                new Dictionary<string, object?>
                {

                    {
                        "errorCode",
                        "PRODUCT_NOT_FOUND"
                    },


                    {
                        "traceId",
                        http.TraceIdentifier
                    }

                });
    }



    return Results.Ok(product);

});



// MVC Route
app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Home}/{action=Index}/{id?}");



app.Run();

