using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using IronSphere.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Python.Runtime;

using wasp.WebApi.Services;
using wasp.WebApi.Services.Configuration;
using wasp.WebApi.Services.DatabaseAccess;
using wasp.WebApi.Services.DataDefinition;
using wasp.WebApi.Services.Environment;

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

            
            PythonSettings pythonSettings = _configuration.GetSection(PythonSettings.SectionName).Get<PythonSettings>();
            Environment.SetEnvironmentVariable(@"PYTHONNET_PYDLL",pythonSettings.PythonDll);
            Runtime.PythonDLL = pythonSettings.PythonDll;
            
            Environment.SetEnvironmentVariable("PATH", pythonSettings.Path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pythonSettings.Path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", $"{pythonSettings.Path}\\Lib\\site-packages;{pythonSettings.Path}\\Lib", EnvironmentVariableTarget.Process);
            
            PythonEngine.Initialize();
            threadState = PythonEngine.BeginAllowThreads();

            services.AddControllers();

            services.AddTransient<IDiContainer, DiContainer>();
            services.AddTransient<IDatabaseService, DatabaseService>();
            services.AddTransient<IDataDefinitionService, SqlServerDataDefinitionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
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
