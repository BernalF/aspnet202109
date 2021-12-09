using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Northwind.Store.Data;
using System;
using Microsoft.AspNetCore.Mvc;
using Northwind.Store.UI.Web.Internet.Settings;

namespace Northwind.Store.UI.Web.Internet
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
            services.AddDbContextPool<NwContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NW")));

            services.AddDistributedMemoryCache();
            // SQL Server, Redis, NCache

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing. The default is 20 minutes.
                options.IdleTimeout = TimeSpan.FromSeconds(120);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddHttpContextAccessor();
            services.AddResponseCaching();

            services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("Basic", new CacheProfile()
                {
                    Duration = 10,
                    VaryByHeader = "User-Agent"
                });
                options.CacheProfiles.Add("NoCaching",
                    new CacheProfile() {NoStore = true, Location = ResponseCacheLocation.None});
            });

            services.AddTransient(typeof(SessionSettings));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                context.Items["StartTime"] = $"Pipeline start {DateTime.Now}";
                await next.Invoke();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseResponseCaching();
            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
