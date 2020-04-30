using IoT.Shared;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Device.Gpio;

namespace IoTVehicle.Api.FakeClasses
{
  public class FakeGpio : IGpio
  {
    private readonly ILogger logger;
    private Dictionary<int, PinValue> pinState = new Dictionary<int, PinValue>();
    private IEnumerable<IPin> pinMapping;

    public FakeGpio(ILogger logger)
    {
      this.logger = logger;
    }

    public void Dispose()
    {
      logger.LogInformation("Fake GPIO object disposed");
    }

    public void Initialize(IEnumerable<IPin> pinMapping)
    {
      logger.LogInformation("Initializing GPIO controller...");
      
      this.pinMapping = pinMapping;

      foreach (var pin in pinMapping)
      {
        pinState.Add(pin.PinNumber, PinValue.Low);
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

    public void SetPin(int pinNumber, PinValue pinValue)
    {
      if (pinNumber == 0)
      {
        logger.LogInformation("I will not set PIN 0 to any value.");
        return;
      }
      pinState[pinNumber] = pinValue;

      logger.LogInformation($"Set value {pinValue} to the PIN number {pinNumber}");
    }
  }
}
