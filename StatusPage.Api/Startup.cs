using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StatusCake.Client;
using StatusCake.Client.Interfaces;
using StatusPage.Api.Config;
using StatusPage.Api.HostedServices;
using StatusPage.Api.Interfaces;
using StatusPage.Api.Models;

namespace StatusPage.Api
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
            // swagger
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "StatusPage - HTTP API",
                    Version = "v1",
                    Description = "The Micro service HTTP API. This is a Data-Driven/CRUD micro service.",
                    TermsOfService = "Terms Of Service"
                });
            });

            // inject statuscake client
            var statusCakeConfig = Configuration.GetSection("StatusCake").Get<StatusCakeConfig>();
            services.AddSingleton<IStatusCakeClient, StatusCakeClient>(s => new StatusCakeClient(statusCakeConfig.UserName, statusCakeConfig.ApiKey));

            // tests data updater
            services.AddHostedService<CakeUpdaterHostedService>();

            // models
            services.AddSingleton<ITestsModel, TestsModel>();
            services.AddSingleton<IAvailabilityModel, AvailabilityModel>();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StatusPage API v1");
                });
            
            app.UseMvc();
        }
    }
}
