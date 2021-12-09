using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Northwind.Store.Data;
using Northwind.Store.Model;
using Northwind.Store.UI.Web.Intranet.Data;

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

            services.AddDbContextPool<NwContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("NW")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IRepository<Category>, BaseRepository<Category>>();
            services.AddTransient<IRepository<Customer>, BaseRepository<Customer>>();
            services.AddTransient<IRepository<Employee>, BaseRepository<Employee>>();
            services.AddTransient<IRepository<Order>, BaseRepository<Order>>();
            services.AddTransient<IRepository<OrderDetail>, BaseRepository<OrderDetail>>();
            services.AddTransient<IRepository<Product>, BaseRepository<Product>>();
            services.AddTransient<IRepository<Region>, BaseRepository<Region>>();
            services.AddTransient<IRepository<Shipper>, BaseRepository<Shipper>>();
            services.AddTransient<IRepository<Supplier>, BaseRepository<Supplier>>();
            services.AddTransient<IRepository<Territory>, BaseRepository<Territory>>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
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
