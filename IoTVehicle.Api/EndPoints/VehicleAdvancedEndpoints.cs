using IoTVehicle.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoTVehicle.Api.EndPoints
{
  public class VehicleAdvancedEndpoints
  {
    public static async Task GoForward(HttpContext context)
    {
      var routeParamExist = Int32.TryParse(context.Request.RouteValues["moveTime"].ToString(), out int moveTime);

      if (!routeParamExist)
      {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync($"Move time parameter is incorrect.");

        return;
      }

      var driveService = GetDriveService(context);

      await driveService.GoForward(moveTime);

      await context.Response.WriteAsync($"Moving forward for {moveTime}.");
    }

    public static async Task GoBackward(HttpContext context)
    {
      var routeParamExist = Int32.TryParse(context.Request.RouteValues["moveTime"].ToString(), out int moveTime);

      if (!routeParamExist)
      {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync($"Move time parameter is incorrect.");

        return;
      }

      var driveService = GetDriveService(context);

      await driveService.GoBackward(moveTime);

      await context.Response.WriteAsync($"Moving backward for {moveTime}.");
    }

    public static async Task TurnLeft(HttpContext context)
    {
      var routeParamExist = Int32.TryParse(context.Request.RouteValues["moveTime"].ToString(), out int moveTime);

      if (!routeParamExist)
      {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync($"Move time parameter is incorrect.");

        return;
      }

      var driveService = GetDriveService(context);

      await driveService.TurnLeft(moveTime);

      await context.Response.WriteAsync($"Moving left for {moveTime}.");
    }

    public static async Task TurnRight(HttpContext context)
    {
      var routeParamExist = Int32.TryParse(context.Request.RouteValues["moveTime"].ToString(), out int moveTime);

      if (!routeParamExist)
      {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync($"Move time parameter is incorrect.");

        return;
      }

      var driveService = GetDriveService(context);

      await driveService.TurnRight(moveTime);

      await context.Response.WriteAsync($"Moving right for {moveTime}.");
    }

    private static IDriveService GetDriveService(HttpContext context)
    {
      var services = context.RequestServices;
      var driveServiceFactory = services.GetService(typeof(IDriveServiceFactory)) as IDriveServiceFactory;
      return driveServiceFactory.GetDriveService();
    }
  }
}
