using DistanceSensor;
using IoTVehicle.Api.Services;
using Microsoft.AspNetCore.Http;
using MotorDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoTVehicle.Api.EndPoints
{
  public class VehicleEndpoints
  {
    public static async Task GoForward(HttpContext context)
    {
      var driveService = GetDriveService(context);

      driveService.GoForward();

      await context.Response.WriteAsync(JsonSerializer.Serialize("Moving forward completed."));
    }

    public static async Task Stop(HttpContext context)
    {
      var driveService = GetDriveService(context);

      driveService.StopDrive();

      await context.Response.WriteAsync(JsonSerializer.Serialize("Full stop"));
    }

    public static async Task TurnLeft(HttpContext context)
    {
      var driveService = GetDriveService(context);

      await driveService.TurnLeft();

      await context.Response.WriteAsync(JsonSerializer.Serialize("Turning left"));
    }

    public static async Task TurnRight(HttpContext context)
    {
      var driveService = GetDriveService(context);

      await driveService.TurnRight();

      await context.Response.WriteAsync(JsonSerializer.Serialize("Turning right"));
    }

    public static async Task GoBackward(HttpContext context)
    {
      var driveService = GetDriveService(context);

      driveService.GoBackward();

      await context.Response.WriteAsync(JsonSerializer.Serialize("Mowing back"));
    }

    public static async Task Index(HttpContext context)
    {
      await context.Response.WriteAsync(JsonSerializer.Serialize("Possible endpoints: GoForward, GoBackWard, TurnLeft, TurnRight"));
    }

    public static async Task MeasureDistance(HttpContext context)
    {
      var services = context.RequestServices;
      var sensorDriver = services.GetService(typeof(IDistanceSensorDriver)) as IDistanceSensorDriver;
      
      var measurement = await sensorDriver.MeasureDistance(default);

      await context.Response.WriteAsync(JsonSerializer.Serialize(measurement.ToString()));
    }

    private static IDriveService GetDriveService(HttpContext context)
    {
      var services = context.RequestServices;
      var driveServiceFactory = services.GetService(typeof(IDriveServiceFactory)) as IDriveServiceFactory;
      return driveServiceFactory.GetDriveService();
    }
  }
}
