using IoTVehicle.Api.PinMappings;
using Microsoft.Extensions.Logging;
using MotorDriver;
using System.Linq;

namespace IoTVehicle.Api.Services
{
  public class DriveServiceFactory : IDriveServiceFactory
  {
    private readonly IMotor motor1;
    private readonly IMotor motor2;
    private readonly IMotorPinMapping motorPinMapping;
    private readonly ILogger<DriveService> logger;

    public DriveServiceFactory(IMotor motor1, IMotor motor2, IMotorPinMapping pinMappingService, ILogger<DriveService> logger)
    {
      this.motor1 = motor1;
      this.motor2 = motor2;
      this.motorPinMapping = pinMappingService;
      this.logger = logger;

      motor1.Initialize(motorPinMapping.CreatePinMapping(1).ToList());
      motor2.Initialize(motorPinMapping.CreatePinMapping(2).ToList());
    }

    public IDriveService GetDriveService()
    {
      return new DriveService(motor1, motor2, logger);
    }
  }
}
