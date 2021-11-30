using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Northwind.Store.Data;
using Northwind.Store.Model;
using Northwind.Store.UI.Web.Intranet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Westwind.AspNetCore.LiveReload;

namespace Northwind.Store.UI.Web.Intranet
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
            //services.AddDbContext<NWContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("NW")));

            services.AddDbContextPool<NWContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("NW")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddLiveReload();

            services.AddControllersWithViews();

            services.AddTransient<IRepository<Category, int>, BaseRepository<Category, int>>();
            services.AddTransient<CategoryRepository>();

            //services.AddScoped
            //services.AddTransient
            //services.AddSingleton
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseLiveReload();
                app.UseDeveloperExceptionPage();                
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseStatusCodePages();
            //app.UseStatusCodePagesWithRedirects("/Home/ErrorWithCode?code={0}");
            app.UseStatusCodePagesWithRedirects("/ErrorStatus.html?code={0}");
            //app.UseStatusCodePagesWithRedirects("/Status{0}.html?code=");
            //app.UseStatusCodePagesWithReExecute("/Home/ErrorWithCode", "?code={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}


// SQL Injection
// Cross-Site Scripting (XSS)
// Cross-Site Request Forgery (XSRF/CSRF)
// Open Redirect Attacks
// Cross-Origin Requests (CORS)