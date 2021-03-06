using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TodoManager.DataAccess.SQLite;
using TodoManager.Web.Pipeline;

namespace TodoManager.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            TodoManager.Implementation.DependencyRegistration.Register(services);
            TodoManager.DataAccess.SQLite.DependencyRegistration.Register(services);
            
            services.AddCors();
            
            services.AddControllers();
            
            services.AddAutoMapper(
                typeof(Startup), 
                typeof(TodoManager.Implementation.DependencyRegistration),
                typeof(TodoManager.DataAccess.SQLite.DependencyRegistration));
            
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            if (env.IsDevelopment())
            {
                app.UseCors(options => options.
                    WithOrigins().AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            }
            
            app.UseSerilogRequestLogging();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = env.ContentRootFileProvider
            });   

            app.ConfigureExceptionHandler();
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}