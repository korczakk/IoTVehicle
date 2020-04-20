using System;
using System.Collections.Generic;
using System.Device.Gpio;

namespace IoT.Shared
{
    public interface IGpio : IDisposable
    {
        void Initialize(IEnumerable<IPinMapping> pinMapping);

        void SetPin(int pinNumber, PinValue pinValue);

        PinValue ReadPin(int pinNumber);
    }
}