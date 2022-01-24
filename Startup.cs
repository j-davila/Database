// Created startup.cs by following the official documentation to migrate ASP.NET Core from 5.0 to 6.0
// https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60?view=aspnetcore-5.0&tabs=visual-studio#where-do-i-put-state-that-was-stored-as-fields-in-my-program-or-startup-class

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Net.Mime;

namespace Database.Startup
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                // xabaril/AspNetCore.Diagnostics.HealthChecks
                endpoints.MapHealthChecks("/hc/ready", new HealthCheckOptions{
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = async(context, report) =>
                    {
                        var result = JsonSerializer.Serialize(
                            new{
                                status = report.Status.ToString(),
                                checks = report.Entries.Select(entry => new {
                                    name = entry.Key,
                                    status = entry.Value.Status.ToString(),
                                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none", // shorthand if/else condition
                                    duration = entry.Value.Duration.ToString()
                                })
                            }
                        );

                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });

                endpoints.MapHealthChecks("/hc/live", new HealthCheckOptions{
                    Predicate = (_) => false
                });
            });
        }
    }
}