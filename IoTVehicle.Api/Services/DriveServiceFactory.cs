using Microsoft.Extensions.Logging;
using MotorDriver;

namespace IoTVehicle.Api.Services
{
  public class DriveServiceFactory : IDriveServiceFactory
  {
    private readonly IMotor motor1;
    private readonly IMotor motor2;
    private readonly IPinMappingService pinMappingService;
    private readonly ILogger logger;

    public DriveServiceFactory(IMotor motor1, IMotor motor2, IPinMappingService pinMappingService, ILogger<IDriveServiceFactory> logger)
    {
      this.motor1 = motor1;
      this.motor2 = motor2;
      this.pinMappingService = pinMappingService;
      this.logger = logger;

      motor1.Initialize(pinMappingService.GetMapping(1));
      motor2.Initialize(pinMappingService.GetMapping(2));
    }

    public IDriveService GetDriveService()
    {
      return new DriveService(motor1, motor2, logger);
    }
  }
}
