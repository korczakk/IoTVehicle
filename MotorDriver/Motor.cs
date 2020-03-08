using Microsoft.Extensions.Logging;
using System.Device.Gpio;

namespace MotorDriver
{
  public class Motor : IMotor
  {
    private IPinMapping pinMapping;
    private readonly ILogger logger;
    private readonly IGpio gpioController;

    public Motor(IGpio gpio, ILogger logger)
    {
      this.logger = logger;
      this.gpioController = gpio;
    }

    public void Initialize(IPinMapping pinMapping)
    {
      this.pinMapping = pinMapping;
    }

    public void StartMotor(RotateDirection direction)
    {
      switch (direction)
      {
        case RotateDirection.ClockWise:
          gpioController.SetPin(pinMapping.PinInput1, PinValue.High);
          gpioController.SetPin(pinMapping.PinInput2, PinValue.Low);
          break;
        case RotateDirection.CounterClockWise:
          gpioController.SetPin(pinMapping.PinInput1, PinValue.Low);
          gpioController.SetPin(pinMapping.PinInput2, PinValue.High);
          break;
        default:
          logger.LogError($"Trying to start engine with unknown direction parameter: {direction.ToString()}");
          break;
      }

      gpioController.SetPin(pinMapping.PinPwm, PinValue.High);
      gpioController.SetPin(pinMapping.PinStandBy, PinValue.High);

      logger.LogInformation($"Starting motor in {direction.ToString()} direction.");
    }

    public void StopMotor()
    {
      gpioController.SetPin(pinMapping.PinInput1, PinValue.Low);
      gpioController.SetPin(pinMapping.PinInput2, PinValue.Low);
      gpioController.SetPin(pinMapping.PinPwm, PinValue.Low);

      logger.LogInformation("Motor has been stopped");
    }
  }
}
