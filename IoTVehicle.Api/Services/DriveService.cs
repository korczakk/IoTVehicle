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

    public void GoForwardLeft()
    {
      motor1.StartClockWise();
    }

    public void GoForwardRight()
    {
      motor2.StartClockWise();
    }

    public void GoBackward()
    {
      motor1.StartCounterClockWise();
      motor2.StartCounterClockWise();

      logger.LogInformation("Moving backward.");
    }

    public void GoBackwardLeft()
    {
      motor1.StartCounterClockWise();
    }

    public void GoBackwardRight()
    {
      motor2.StartCounterClockWise();
    }

    public async Task TurnLeft()
    {
      motor1.StartCounterClockWise();

      motor2.StartClockWise();

      await Task.Delay(TimeSpan.FromMilliseconds(1250));

      motor1.StartClockWise();

      logger.LogInformation("Turned left.");
    }

    public async Task TurnRight()
    {
      motor2.StartCounterClockWise();

      motor1.StartClockWise();

      await Task.Delay(TimeSpan.FromMilliseconds(1250));

      motor2.StartClockWise();

      logger.LogInformation("Turned right.");
    }

    public void StopDrive()
    {
      motor1.Stop();
      motor2.Stop();

      logger.LogInformation("All stop.");
    }

    public void StopLeftDrive()
    {
      motor1.Stop();
    }

    public void StopRightDrive()
    {
      motor2.Stop();
    }


    /// <summary>
    /// Vehicle is moving forward if one of the motors is spinnign clockwise.
    /// </summary>
    /// <returns></returns>
    public bool IsGoingForward()
    {
      MotorState motor1State = motor1.GetMotorState();
      MotorState motor2State = motor2.GetMotorState();

      return motor1State == MotorState.SpinningClockWise || motor2State == MotorState.SpinningClockWise;
    }
  }
}
