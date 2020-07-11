using IoTVehicle.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace IoTVehicle.Api.SignalRHubs
{
  public class VehicleControlHub : Hub
  {
    public void GoForward()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.GoForward();      
    }

    public void Stop()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.StopDrive();
    }

    public async Task TurnLeft()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      await driveService.TurnLeft();
    }

    public async Task TurnRight()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      await driveService.TurnRight();
    }

    public void GoBackward()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.GoBackward();
    }

    private IDriveService GetDriveService(HttpContext context)
    {
      var services = context.RequestServices;
      var driveServiceFactory = services.GetService(typeof(IDriveServiceFactory)) as DriveServiceFactory;

      return driveServiceFactory.GetDriveService();
    }
  }
}
