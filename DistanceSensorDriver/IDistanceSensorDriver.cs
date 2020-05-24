using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistanceSensor
{
  public interface IDistanceSensorDriver
  {
    Task<double> MeasureDistance();
  }
}