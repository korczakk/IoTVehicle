using IoTVehicle.Api.Services;
using Microsoft.AspNetCore.Http;
using MotorDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTVehicle.Api.EndPoints
{
  public class VehicleEndpoints
  {
    public static async Task GoForward(HttpContext context)
    {
      var driveService = GetDriveService(context);

      driveService.GoForward();

      await context.Response.WriteAsync("Moving forward completed.");
    }

    public static async Task Stop(HttpContext context)
    {
      var driveService = GetDriveService(context);

      driveService.StopDrive();

      await context.Response.WriteAsync("Full stop");
    }

    public static async Task TurnLeft(HttpContext context)
    {
      var driveService = GetDriveService(context);

      await driveService.TurnLeft();

      await context.Response.WriteAsync("Turning left");
    }

    public static async Task TurnRight(HttpContext context)
    {
      var driveService = GetDriveService(context);

      await driveService.TurnRight();

      await context.Response.WriteAsync("Turning right");
    }

    public static async Task GoBackward(HttpContext context)
    {
      var driveService = GetDriveService(context);

      driveService.GoBackward();

      await context.Response.WriteAsync("Mowing back");
    }

    public static async Task Index(HttpContext context)
    {
      await context.Response.WriteAsync("Possible endpoints: GoForward, GoBackWard, TurnLeft, TurnRight");
    }

    private static IDriveService GetDriveService(HttpContext context)
    {
      var services = context.RequestServices;
      var driveServiceFactory = services.GetService(typeof(IDriveServiceFactory)) as IDriveServiceFactory;
      return driveServiceFactory.GetDriveService();
    }
  }
}
