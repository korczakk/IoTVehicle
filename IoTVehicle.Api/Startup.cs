using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoTVehicle.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MotorDriver;

namespace IoTVehicle.Api
{
  public class Startup
  {
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddLogging(x => x.AddConsole());
      services.AddSingleton<IPinMappingService>(sp =>
      {
        var mappingService = new PinMappingService(sp.GetService<ILogger<PinMappingService>>(), configuration);
        mappingService.CreateMappings();

        return mappingService;
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGet("/", async context =>
              {
                var serv = context.RequestServices;
                var mappingService = serv.GetService<IPinMappingService>();

            await context.Response.WriteAsync(mappingService.GetAllMappings().Count().ToString());
          });
      });
    }
  }
}
