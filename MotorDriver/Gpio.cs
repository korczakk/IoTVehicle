using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;

namespace MotorDriver
{
    public class Gpio : IGpio
    {
        private readonly GpioController controller;
        private readonly ILogger logger;
        private IEnumerable<IPinMapping> pinMapping;

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
            controller.Write(pinNumber, pinValue);
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
            return controller.Read(pinNumber);
        }

        private void OpenPin(int pinNumber)
        {
            if (pinNumber == 0)
                return;

            controller.OpenPin(pinNumber, PinMode.Output);
        }
    }
}
