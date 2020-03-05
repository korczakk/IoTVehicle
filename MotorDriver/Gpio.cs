using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;

namespace MotorDriver
{
  public class Gpio : IGpio, IDisposable
  {
    private readonly ILogger logger;
    private IEnumerable<IPinMapping> pinMapping;

    public Gpio(ILogger logger)
    {
      controller = new GpioController();
      this.logger = logger;
    }

    public GpioController controller { get; }

    public void Initialize(IEnumerable<IPinMapping> pinMapping)
    {
      logger.LogInformation("Initializing GPIO controller...");

      if(pinMapping is null && pinMapping.Count() == 0)
      {
        logger.LogError("PinMapping is empty");
        throw new ArgumentNullException(nameof(pinMapping));
      }

      this.pinMapping = pinMapping;

      OpenPins();
    }

    public void Dispose()
    {
      if(controller is { })
      {
        controller.Dispose();
      }
    }

    private void OpenPins()
    {
      foreach (var pin in pinMapping)
      {
        controller.OpenPin(pin.PinInput1, PinMode.Output);
        controller.OpenPin(pin.PinInput2, PinMode.Output);
        controller.OpenPin(pin.PinPwm, PinMode.Output);
        controller.OpenPin(pin.PinStandBy, PinMode.Output);
      }

      logger.LogInformation($"Opened pins from {pinMapping.Count()} mappings.");
    }
  }
}
