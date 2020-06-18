using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistanceSensor;
using IoT.Shared;
using IoTVehicle.Api.EndPoints;
using IoTVehicle.Api.FakeClasses;
using IoTVehicle.Api.PinMappings;
using IoTVehicle.Api.Services;
using IoTVehicle.Api.Workers;
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
      services.AddTransient<IMotorPinMapping>(sp =>
      {
        return new MotorPinMapping(configuration);
      });
      services.AddTransient<IDistanceSensorPinMapping>(sp => new DistanceSensorPinMapping(configuration));
      services.AddSingleton<IGpio>(sp =>
      {
        var gpio = new FakeGpio(sp.GetService<ILogger<Gpio>>());
        var motorPinMapping = sp.GetService<IMotorPinMapping>();
        var distanceSensorPinMapping = sp.GetService<IDistanceSensorPinMapping>();

        var mappings = new List<IPin>();
        mappings.AddRange(motorPinMapping.CreatePinMapping(1));
        mappings.AddRange(motorPinMapping.CreatePinMapping(2));
        mappings.AddRange(distanceSensorPinMapping.CreatePinMapping());

        gpio.Initialize(mappings);

        return gpio;
      });
      services.AddTransient<IMotor>(sp =>
      {
        var gpio = sp.GetService<IGpio>();
        var logger = sp.GetService<ILogger<Motor>>();

        return new Motor(gpio, logger);
      });
      services.AddTransient<IDriveServiceFactory>(sp =>
      {
        var logger = sp.GetService<ILogger<DriveService>>();
        var motor1 = sp.GetService<IMotor>();
        var motor2 = sp.GetService<IMotor>();
        var pinMappingService = sp.GetService<IMotorPinMapping>();

        return new DriveServiceFactory(motor1, motor2, pinMappingService, logger);
      });
      services.AddTransient<IDistanceSensorDriver>(sp =>
      {
        var logger = sp.GetService<ILogger<DistanceSensorDriver>>();
        var gpio = sp.GetService<IGpio>();
        var pinMapping = sp.GetService<IDistanceSensorPinMapping>();

        return new DistanceSensorDriver(gpio, logger, pinMapping.CreatePinMapping());
      });

      services.AddHostedService<DistanceMeasurementWorker>(sp =>
      {
        var logger = sp.GetService<ILogger<DistanceMeasurementWorker>>();
        var distanceSensor = sp.GetService<IDistanceSensorDriver>();
        var driverServiceFactory = sp.GetService<IDriveServiceFactory>();

        return new DistanceMeasurementWorker(logger, distanceSensor, driverServiceFactory);
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGet("/", VehicleEndpoints.Index);
        endpoints.MapGet("/goforward", VehicleEndpoints.GoForward);
        endpoints.MapGet("/gobackward", VehicleEndpoints.GoBackward);
        endpoints.MapGet("/stop", VehicleEndpoints.Stop);
        endpoints.MapGet("/turnleft", VehicleEndpoints.TurnLeft);
        endpoints.MapGet("/turnright", VehicleEndpoints.TurnRight);
        endpoints.MapGet("/getdistance", VehicleEndpoints.MeasureDistance);
        endpoints.MapPost("/advancedcontrol/goforward/{moveTime}", VehicleAdvancedEndpoints.GoForward);
        endpoints.MapPost("/advancedcontrol/gobackward/{moveTime}", VehicleAdvancedEndpoints.GoBackward);
        endpoints.MapPost("/advancedcontrol/turnleft/{moveTime}", VehicleAdvancedEndpoints.TurnLeft);
        endpoints.MapPost("/advancedcontrol/turnright/{moveTime}", VehicleAdvancedEndpoints.TurnRight);
      });

      appLifetime.ApplicationStopping.Register(OnShuttingDown, app.ApplicationServices);
    }

    private void OnShuttingDown(Object serviceProvider)
    {
      var gpio = ((IServiceProvider)serviceProvider).GetService<IGpio>();
      gpio.Dispose();
    }
  }
}
