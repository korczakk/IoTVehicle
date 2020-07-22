using DistanceSensor;
using IoT.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IoTVehicle.Api.FakeClasses
{
  public class FakeDistanceSensor : IDistanceSensorDriver
  {
    public Channel<double> MeasurementsChannel { get; }
    private double distance = 0.0;

    public FakeDistanceSensor(IGpio gpio, ILogger logger, IEnumerable<DistanceSensorPin> pinMapping)
    {
      MeasurementsChannel = Channel.CreateBounded<double>(new BoundedChannelOptions(21)
      {
        FullMode = BoundedChannelFullMode.DropOldest
      });
    }

    public async Task<double> MeasureDistance(CancellationToken ct)
    {
      if (distance >= 200)
      {
        distance = 0;
      }

      distance++;

      await MeasurementsChannel.Writer.WriteAsync(distance, ct);

      return distance;
    }
  }
}
