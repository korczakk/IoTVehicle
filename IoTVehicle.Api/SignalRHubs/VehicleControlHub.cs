using DistanceSensor;
using IoTVehicle.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoTVehicle.Api.SignalRHubs
{
  public class VehicleControlHub : Hub
  {
    private readonly IDistanceSensorDriver distanceSensorDriver;

    public VehicleControlHub(IDistanceSensorDriver distanceSensorDriver)
    {
      this.distanceSensorDriver = distanceSensorDriver;
    }

    public void GoForward()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.GoForward();      
    }

    public void GoLeftForward()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.GoForwardLeft();
    }

    public void GoRightForward()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.GoForwardRight();
    }

    public void Stop()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.StopDrive();
    }

    public void StopLeft()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.StopLeftDrive();
    }

    public void StopRight()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.StopRightDrive();
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

    public void GoLeftBackward()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.GoBackwardLeft();
    }

    public void GoRightBackward()
    {
      var driveService = GetDriveService(Context.GetHttpContext());

      driveService.GoBackwardRight();
    }

    public IAsyncEnumerable<double> GetDistanceMeasurement()
    {
      return distanceSensorDriver.MeasurementsChannel.Reader.ReadAllAsync();
    }

    private IDriveService GetDriveService(HttpContext context)
    {
      var services = context.RequestServices;
      var driveServiceFactory = services.GetService(typeof(IDriveServiceFactory)) as DriveServiceFactory;

      return driveServiceFactory.GetDriveService();
    }
  }
}
