using Microsoft.Extensions.Logging;
using System;
using System.Device.Gpio;

namespace MotorDriver
{
  public class Motor : IDisposable
  {
    private IPinMapping pinMapping;
    private readonly ILogger logger;
    private readonly GpioController controller;

    public Motor(IGpio gpio, ILogger logger)
    {
      this.logger = logger;
      this.controller = gpio.controller;
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
          controller.Write(pinMapping.PinInput1, PinValue.High);
          controller.Write(pinMapping.PinInput2, PinValue.Low);
          break;
        case RotateDirection.CounterClockWise:
          controller.Write(pinMapping.PinInput1, PinValue.Low);
          controller.Write(pinMapping.PinInput2, PinValue.High);
          break;
        default:
          logger.LogError($"Trying to start engine with unknown direction parameter: {direction}");
          break;
      }
      
      controller.Write(pinMapping.PinPwm, PinValue.High);
      controller.Write(pinMapping.PinStandBy, PinValue.High);

      logger.LogInformation($"Starting motor in {direction} direction.");
    }

    public void StopMotor()
    {
      controller.Write(pinMapping.PinInput1, PinValue.Low);
      controller.Write(pinMapping.PinInput2, PinValue.Low);
      controller.Write(pinMapping.PinPwm, PinValue.Low);

      logger.LogInformation("Motor has been stopped");
    }

    public void Dispose()
    {
      controller.Dispose();
    }
  }
}
