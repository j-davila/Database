// Created startup.cs by following the official documentation to migrate ASP.NET Core from 5.0 to 6.0
// https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60?view=aspnetcore-5.0&tabs=visual-studio#where-do-i-put-state-that-was-stored-as-fields-in-my-program-or-startup-class

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
            });
        }
    }
}