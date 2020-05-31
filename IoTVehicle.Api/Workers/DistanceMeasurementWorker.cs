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
  public class DistanceMeasurementWorker : IHostedService
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

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        double distance = await this.distanceSensor.MeasureDistance();

        // TODO: add condition checking if vehicle goes forward
        if (distance <= 10 && driveService.IsGoingForward())
        {
          logger.LogInformation("Distance less than 15 cm. Stopping.");

          driveService.StopDrive();
        }

        await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
      }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }
  }
}
