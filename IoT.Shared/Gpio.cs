using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;

namespace IoT.Shared
{
  public class Gpio : IGpio
  {
    private readonly GpioController controller;
    private readonly ILogger logger;
    private IEnumerable<IPinMapping> pinMapping;
    private Dictionary<int, PinValue> pinState = new Dictionary<int, PinValue>();

    public Gpio(ILogger logger)
    {
      controller = new GpioController();
      this.logger = logger;
    }

    public void Initialize(IEnumerable<IPinMapping> pinMapping)
    {
      logger.LogInformation("Initializing GPIO controller...");

      if (pinMapping is null || pinMapping.Count() == 0)
      {
        logger.LogError("PinMapping is empty");
        throw new ArgumentNullException(nameof(pinMapping));
      }

      this.pinMapping = pinMapping;

      foreach (var pin in pinMapping)
      {
        OpenPin(pin.PinInput1);
        OpenPin(pin.PinInput2);
        OpenPin(pin.PinPwm);
        OpenPin(pin.PinStandBy);
      }

      logger.LogInformation($"Opened pins from {pinMapping.Count()} mappings.");
    }

    public void SetPin(int pinNumber, PinValue pinValue)
    {
      if (pinNumber == 0)
      {
        logger.LogInformation("I will not set PIN 0 to any value.");
        return;
      }

      controller.Write(pinNumber, pinValue);
      pinState[pinNumber] = pinValue;

      logger.LogInformation($"PIN {pinNumber} set to value {pinValue}");
    }

    public void Dispose()
    {
      if (controller is { })
      {
        controller.Dispose();
      }
    }

    public PinValue ReadPin(int pinNumber)
    {
      if (pinNumber == 0)
      {
        logger.LogInformation("I will not read from PIN 0 to any value.");
        return PinValue.Low;
      }

      return pinState[pinNumber];
    }

    private void OpenPin(int pinNumber)
    {
      if (pinNumber == 0)
        return;

      controller.OpenPin(pinNumber, PinMode.Output);
      pinState.Add(pinNumber, PinValue.Low);
    }
  }
}
