using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DistanceSensor
{
  public interface IDistanceSensorDriver
  {
    Task<double> MeasureDistance(CancellationToken ct);

    Channel<double> MeasurementsChannel { get; }
  }
}