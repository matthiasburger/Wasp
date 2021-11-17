using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Python.Runtime;

using Serilog;

using wasp.WebApi.Services;
using wasp.WebApi.Services.Configuration;
using wasp.WebApi.Services.DatabaseAccess;
using wasp.WebApi.Services.DataDefinition;
using wasp.WebApi.Services.Environment;
using wasp.WebApi.Services.PythonEngine;
using wasp.WebApi.Services.PythonLogger;

namespace wasp.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _environmentDiscovery = new EnvironmentDiscovery();
        }

        private readonly IConfiguration _configuration;
        private readonly IEnvironmentDiscovery _environmentDiscovery;

        private IntPtr threadState;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PythonSettings>(_configuration.GetSection("PythonSettings"));

            threadState = services.AddPython(_configuration);

            services.AddControllers();

            services.AddTransient<IDiContainer, DiContainer>();
            services.AddTransient<IPythonLogger, PythonLogger>();
            services.AddTransient<IPythonEngine, PythonNetEngine>();
            services.AddTransient<IDatabaseService, DatabaseService>();
            services.AddTransient<IDataDefinitionService, SqlServerDataDefinitionService>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days.
                // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            loggerFactory.AddSerilog();
            
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            applicationLifetime.ApplicationStopping.Register(OnShutdown);
        }

        public void OnShutdown()
        {
            PythonEngine.EndAllowThreads(threadState);
            PythonEngine.Shutdown();
        }
    }
}
