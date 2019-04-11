using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StatusCake.Client;
using StatusCake.Client.Interfaces;
using StatusPage.HostedServices;
using StatusPage.Data;
using StatusPage.Interfaces;
using StatusPage.Models;

namespace StatusPage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // inject statuscake client
            var statusCakeUsername = Configuration.GetSection("StatusCake").GetValue<string>("Username");
            var statusCakeApiKey = Configuration.GetSection("StatusCake").GetValue<string>("ApiKey");
            services.AddSingleton<IStatusCakeClient, StatusCakeClient>(s => new StatusCakeClient(statusCakeUsername, statusCakeApiKey));

            // for live tests data
            services.AddHostedService<TestsUpdaterHostedService>();
            services.AddSingleton<ITestsModel, TestsModel>();

            // db context
            services.AddDbContext<StatusPageContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("ConnectionString")));

            // Background worker db pusher
            services.AddHostedService<DatabaseHostedService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
