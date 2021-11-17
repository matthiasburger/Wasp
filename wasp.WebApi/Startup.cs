using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Python.Runtime;

using wasp.WebApi.Services;
using wasp.WebApi.Services.DatabaseAccess;
using wasp.WebApi.Services.DataDefinition;

namespace wasp.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IntPtr threadState;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            applicationLifetime.ApplicationStopping.Register(OnShutdown);
        }

        public void OnShutdown()
        {
            PythonEngine.EndAllowThreads(threadState);
            PythonEngine.Shutdown();
        }
    }
}
