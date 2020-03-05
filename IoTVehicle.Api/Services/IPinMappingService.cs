using MotorDriver;
using System.Collections.Generic;

namespace IoTVehicle.Api.Services
{
  public interface IPinMappingService
  {
    void CreateMappings();
    IEnumerable<IPinMapping> GetAllMappings();
    IPinMapping GetMapping(int motorNumber);
  }
}