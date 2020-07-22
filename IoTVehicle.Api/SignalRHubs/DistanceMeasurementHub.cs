using DistanceSensor;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Channels;

namespace IoTVehicle.Api.SignalRHubs
{
  public class DistanceMeasurementHub : Hub
  {
    private readonly IDistanceSensorDriver distanceSensorDriver;

    public DistanceMeasurementHub(IDistanceSensorDriver distanceSensorDriver)
    {
      this.distanceSensorDriver = distanceSensorDriver;
    }

    public IAsyncEnumerable<double> GetDistanceMeasurement()
    {
      return distanceSensorDriver.MeasurementsChannel.Reader.ReadAllAsync();
    }
  }
}
