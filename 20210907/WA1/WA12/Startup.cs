using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WA12
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            #region Custom default
            //DefaultFilesOptions options = new DefaultFilesOptions();
            //System.Diagnostics.Debug.WriteLine(options.DefaultFileNames.Aggregate("", (result, next) => { return $"{result}, {next}"; }));
            //options.DefaultFileNames.Clear();
            //options.DefaultFileNames.Add("main.html");
            //app.UseDefaultFiles(options);
            #endregion

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
