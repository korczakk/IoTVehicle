using Microsoft.AspNetCore.Rewrite;
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

    /// <summary>
    /// </summary>
    /// <param name="moveTime">In milliseconds</param>
    public async Task GoForward(int moveTime)
    {
      motor1.StartClockWise();
      motor2.StartClockWise();

      await Task.Delay(TimeSpan.FromMilliseconds(moveTime));

      motor1.Stop();
      motor2.Stop();
    }

    public void GoBackward()
    {
      motor1.StartCounterClockWise();
      motor2.StartCounterClockWise();

      logger.LogInformation("Moving backward.");
    }

    public async Task GoBackward(int moveTime)
    {
      motor1.StartCounterClockWise();
      motor2.StartCounterClockWise();

      await Task.Delay(TimeSpan.FromMilliseconds(moveTime));

      motor1.Stop();
      motor2.Stop();
    }

    public async Task TurnLeft()
    {
      //motor1.Stop();
      motor1.StartCounterClockWise();

      motor2.StartClockWise();

      await Task.Delay(TimeSpan.FromMilliseconds(1250));

      motor1.StartClockWise();

      logger.LogInformation("Turned left.");
    }

    public async Task TurnLeft(int moveTime)
    {
      motor2.StartClockWise();

      await Task.Delay(TimeSpan.FromMilliseconds(moveTime));

      motor2.Stop();
    }

    public async Task TurnRight()
    {
      // motor2.Stop();
      motor2.StartCounterClockWise();

      motor1.StartClockWise();

      await Task.Delay(TimeSpan.FromMilliseconds(1250));

      motor2.StartClockWise();

      logger.LogInformation("Turned right.");
    }

    public async Task TurnRight(int moveTime)
    {
      motor1.StartClockWise();

      await Task.Delay(TimeSpan.FromMilliseconds(moveTime));

      motor1.Stop();
    }

    public void StopDrive()
    {
      motor1.Stop();
      motor2.Stop();

      logger.LogInformation("All stop.");
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
