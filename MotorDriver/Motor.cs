using Microsoft.Extensions.Logging;
using System;
using System.Device.Gpio;

namespace MotorDriver
{
  public class Motor : IMotor
  {
    private IPinMapping pinMapping;
    private readonly ILogger logger;
    private readonly IGpio gpioController;
    private MotorState motorState;

    public Motor(IGpio gpio, ILogger logger)
    {
      if (gpio is null)
      {
        throw new ArgumentNullException("GPIO controller can not be null.");
      }

      this.logger = logger;
      this.gpioController = gpio;
    }

    public void Initialize(IPinMapping pinMapping)
    {
      if (pinMapping is null)
      {
        throw new ArgumentNullException("PinMapping can not be null.");
      }

      this.pinMapping = pinMapping;
      this.motorState = GetMotorState();
    }

    /// <summary>
    /// When the motor state is different than spinning clockwise then it first stops motor and then starts in clockwise
    /// direction.
    /// </summary>
    public void StartClockWise()
    {
      if (motorState != MotorState.SpinningClockWise)
      {
        Stop();

        gpioController.SetPin(pinMapping.PinInput1, PinValue.High);
        gpioController.SetPin(pinMapping.PinInput2, PinValue.Low);

        gpioController.SetPin(pinMapping.PinPwm, PinValue.High);
        gpioController.SetPin(pinMapping.PinStandBy, PinValue.High);

        logger.LogInformation("Starting motor in clockwise direction.");
      }
    }

    /// <summary>
    /// When the motor state is different than spinning counter-clockwise then it first stops motor and then starts in 
    /// counter-clockwise direction.
    /// </summary>
    public void StartCounterClockWise()
    {
      if (motorState != MotorState.SpinningCounterClockWise)
      {
        Stop();

        gpioController.SetPin(pinMapping.PinInput1, PinValue.Low);
        gpioController.SetPin(pinMapping.PinInput2, PinValue.High);

        gpioController.SetPin(pinMapping.PinPwm, PinValue.High);
        gpioController.SetPin(pinMapping.PinStandBy, PinValue.High);

        logger.LogInformation("Starting motor in counter-clockwise direction.");
      }
    }

    public void Stop()
    {
      gpioController.SetPin(pinMapping.PinInput1, PinValue.Low);
      gpioController.SetPin(pinMapping.PinInput2, PinValue.Low);
      gpioController.SetPin(pinMapping.PinPwm, PinValue.Low);

      logger.LogInformation("Motor has been stopped");
    }

    private MotorState GetMotorState()
    {
      var input1 = this.gpioController.ReadPin(pinMapping.PinInput1);
      var input2 = this.gpioController.ReadPin(pinMapping.PinInput2);
      var pwm = this.gpioController.ReadPin(pinMapping.PinPwm);

      if (input1 == PinValue.High && input2 == PinValue.Low && pwm == PinValue.High)
      {
        return MotorState.SpinningClockWise;
      }
      else if (input1 == PinValue.Low && input2 == PinValue.High && pwm == PinValue.High)
      {
        return MotorState.SpinningCounterClockWise;
      }

      return MotorState.Stopped;
    }
  }
}
