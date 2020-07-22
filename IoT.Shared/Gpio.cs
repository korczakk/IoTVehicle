﻿using Microsoft.Extensions.Logging;
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
    private IEnumerable<IPin> pinMapping;
    private Dictionary<int, PinValue> pinState = new Dictionary<int, PinValue>();

    public Gpio(ILogger logger)
    {
      controller = new GpioController();
      this.logger = logger;
    }

    public void Initialize(IEnumerable<IPin> pinMapping)
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
        OpenPin(pin);
      }

      logger.LogDebug($"Opened pins from {pinMapping.Count()} mappings.");
    }

    public void SetPin(int pinNumber, PinValue pinValue)
    {
      if (pinNumber == 0)
      {
        logger.LogDebug("I will not set PIN 0 to any value.");
        return;
      }

      controller.Write(pinNumber, pinValue);
      pinState[pinNumber] = pinValue;

      logger.LogDebug($"PIN {pinNumber} set to value {pinValue}");
    }

    public void Dispose()
    {
      if (controller is { })
      {
        controller.Dispose();
      }
    }

    /// <summary>
    /// Gets Pin state stored in object state. This is not a real pin value read
    /// from device.
    /// </summary>
    /// <param name="pinNumber"></param>
    /// <returns></returns>
    public PinValue CheckPinState(int pinNumber)
    {
      if (pinNumber == 0)
      {
        logger.LogDebug("I will not read from PIN 0.");
        return PinValue.Low;
      }

      return pinState[pinNumber];
    }

    public PinValue ReadPin(int pinNumber)
    {
      return controller.Read(pinNumber);
    }

    private void OpenPin(IPin pin)
    {
      if (pin.PinNumber == 0)
        return;

      controller.OpenPin(pin.PinNumber, pin.Mode);
      pinState.Add(pin.PinNumber, PinValue.Low);
    }
  }
}
