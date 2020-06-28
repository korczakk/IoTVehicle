using DistanceSensor;
using IoTVehicle.Api.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IoTVehicle.Api.Workers
{
  public class DistanceMeasurementWorker : BackgroundService
  {
    private readonly ILogger logger;
    private IDistanceSensorDriver distanceSensor;
    private IDriveService driveService;

    public DistanceMeasurementWorker(ILogger<DistanceMeasurementWorker> logger,
      IDistanceSensorDriver distanceSensor,
      IDriveServiceFactory driverServiceFactory)
    {
      this.logger = logger;
      this.distanceSensor = distanceSensor;

      driveService = driverServiceFactory.GetDriveService();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        double distance = await this.distanceSensor.MeasureDistance();

        if (distance <= 10 && driveService.IsGoingForward())
        {
          logger.LogInformation("Distance less than 15 cm. Stopping.");

          driveService.StopDrive();
        }

        await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken);
      }
    }
  }
}
