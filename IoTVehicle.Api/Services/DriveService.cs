using Microsoft.Extensions.Logging;
using MotorDriver;

namespace IoTVehicle.Api.Services
{
    public class DriveService
    {
        private readonly IMotor motor1;
        private readonly IMotor motor2;
        private readonly ILogger logger;

        public DriveService(IMotor motor1, IMotor motor2, ILogger logger)
        {
            this.motor1 = motor1;
            this.motor2 = motor2;
            this.logger = logger;
        }

        public void GoForward()
        {
            motor1.StopMotor();
            motor2.StopMotor();

            motor1.StartMotor(RotateDirection.ClockWise);
            motor2.StartMotor(RotateDirection.ClockWise);

            logger.LogInformation("Moving forward.");
        }

        public void GoBackward()
        {
            motor1.StopMotor();
            motor2.StopMotor();

            motor1.StartMotor(RotateDirection.CounterClockWise);
            motor2.StartMotor(RotateDirection.CounterClockWise);

            logger.LogInformation("Moving backward.");
        }

        public void TurnLeft()
        {
            // stop motor2
            // start motor1
            // wait for 2 sec
            // start motor1 to move on boath wheels

            logger.LogInformation("Turning left.");
        }

        public void TurnRight()
        {

        }

        public void StopDrive()
        {
            motor1.StopMotor();
            motor2.StopMotor();

            logger.LogInformation("All stop.");
        }
    }
}
