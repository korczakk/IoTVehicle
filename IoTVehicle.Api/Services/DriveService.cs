using Microsoft.Extensions.Logging;
using MotorDriver;
using System;
using System.Threading.Tasks;

namespace IoTVehicle.Api.Services
{
  public class DriveService : IDriveService
  {
    private readonly IMotor motor1;
    private readonly IMotor motor2;
    private readonly ILogger<DriveService> logger;

    public DriveService(IMotor motor1, IMotor motor2, ILogger<DriveService> logger)
    {
      this.motor1 = motor1;
      this.motor2 = motor2;
      this.logger = logger;
    }

    public void GoForward()
    {
      motor1.StartClockWise();
      motor2.StartClockWise();

      logger.LogInformation("Moving forward.");
    }

    public void GoBackward()
    {
      motor1.StartCounterClockWise();
      motor2.StartCounterClockWise();

      logger.LogInformation("Moving backward.");
    }

    public async Task TurnLeft()
    {
      motor1.Stop();

      motor2.StartClockWise();

      await Task.Delay(TimeSpan.FromMilliseconds(250));

      motor1.StartClockWise();

      logger.LogInformation("Turned left.");
    }

    public async Task TurnRight()
    {
      motor2.Stop();

      motor1.StartClockWise();

      await Task.Delay(TimeSpan.FromMilliseconds(250));

      motor2.StartClockWise();

      logger.LogInformation("Turned right.");
    }

    public void StopDrive()
    {
      motor1.Stop();
      motor2.Stop();

      logger.LogInformation("All stop.");
    }

    public bool IsGoingForward()
    {

    }
  }
}
