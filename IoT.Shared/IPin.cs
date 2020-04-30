using System;
using System.Device.Gpio;

namespace IoT.Shared
{
  public interface IPin
  {
    PinMode Mode { get; }
    string Name { get; }
    int PinNumber { get; }
  }
}