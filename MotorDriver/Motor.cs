using IoT.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;

namespace MotorDriver
{
  public class Motor : IMotor
  {
    private IDictionary<MotorPinNames, MotorPin> pinMapping;
    private readonly ILogger logger;
    private readonly IGpio gpioController;
    private MotorState motorState;

    public Motor(IGpio gpio, ILogger<Motor> logger)
    {
      if (gpio is null)
      {
        throw new ArgumentNullException("GPIO controller can not be null.");
      }

      this.logger = logger;
      this.gpioController = gpio;
    }

    public void Initialize(IEnumerable<MotorPin> pinMapping)
    {
      if (pinMapping is null)
      {
        throw new ArgumentNullException("PinMapping can not be null.");
      }

      this.pinMapping = pinMapping.ToDictionary(x => x.PinName);
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

        gpioController.SetPin(pinMapping[MotorPinNames.Input1].PinNumber, PinValue.High);
        gpioController.SetPin(pinMapping[MotorPinNames.Input2].PinNumber, PinValue.Low);

        gpioController.SetPin(pinMapping[MotorPinNames.Pwm].PinNumber, PinValue.High);
        // gpioController.SetPin(pinMapping[MotorPinNames.StandBy].PinNumber, PinValue.High);

        logger.LogInformation($"Starting motor in clockwise direction on {DateTime.Now.ToString("hh:mm:ss.fff")}.");
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

        gpioController.SetPin(pinMapping[MotorPinNames.Input1].PinNumber, PinValue.Low);
        gpioController.SetPin(pinMapping[MotorPinNames.Input2].PinNumber, PinValue.High);

        gpioController.SetPin(pinMapping[MotorPinNames.Pwm].PinNumber, PinValue.High);
        // gpioController.SetPin(pinMapping[MotorPinNames.StandBy].PinNumber, PinValue.High);

        logger.LogInformation("Starting motor in counter-clockwise direction.");
      }
    }

    public void Stop()
    {
      gpioController.SetPin(pinMapping[MotorPinNames.Input1].PinNumber, PinValue.Low);
      gpioController.SetPin(pinMapping[MotorPinNames.Input2].PinNumber, PinValue.Low);
      gpioController.SetPin(pinMapping[MotorPinNames.Pwm].PinNumber, PinValue.Low);
      gpioController.SetPin(pinMapping[MotorPinNames.StandBy].PinNumber, PinValue.Low);

      logger.LogInformation("Motor has been stopped");
    }

    public MotorState GetMotorState()
    {
      var input1 = this.gpioController.CheckPinState(pinMapping[MotorPinNames.Input1].PinNumber);
      var input2 = this.gpioController.CheckPinState(pinMapping[MotorPinNames.Input2].PinNumber);
      var pwm = this.gpioController.CheckPinState(pinMapping[MotorPinNames.Pwm].PinNumber);

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
