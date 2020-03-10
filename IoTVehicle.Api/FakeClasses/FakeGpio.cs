using Microsoft.Extensions.Logging;
using MotorDriver;
using System.Collections.Generic;
using System.Device.Gpio;

namespace IoTVehicle.Api.FakeClasses
{
    public class FakeGpio : IGpio
    {
        private readonly ILogger logger;

        public FakeGpio(ILogger logger)
        {
            this.logger = logger;
        }

        public void Dispose()
        {
            logger.LogInformation("Fake GPIO object disposed");
        }

        public void Initialize(IEnumerable<IPinMapping> pinMapping)
        {
            logger.LogInformation("Fake GPIO object has been initialized");
        }

        public PinValue ReadPin(int pinNumber)
        {
            return PinValue.High;
        }

        public void SetPin(int pinNumber, PinValue pinValue)
        {
            logger.LogInformation($"Set value {pinValue} to the PIN number {pinNumber}");
        }
    }
}
