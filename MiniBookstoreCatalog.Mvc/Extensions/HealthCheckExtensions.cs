using Microsoft.AspNetCore.Diagnostics.HealthChecks;

public static class HealthCheckExtensions
{

    public static IApplicationBuilder
    UseCustomHealthCheck(
    this IApplicationBuilder app)
    {


        app.UseHealthChecks(
        "/health/ready",
        new HealthCheckOptions
        {

            ResponseWriter = async (context, report) =>
    {


            await context.Response
    .WriteAsJsonAsync(new
        {

            status =
    report.Status.ToString(),

            checks =
    report.Entries.Select(x => new
        {

            name = x.Key,

            status = x.Value.Status.ToString()

        })

        });


        }

        });


        return app;

    }

}