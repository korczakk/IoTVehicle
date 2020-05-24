using IoT.Shared;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;

namespace DistanceSensor
{
  public class DistanceSensorPin : IPin
  {
    public PinMode Mode { get; }

    public string Name { get; }

    public int PinNumber { get; }

    public DistanceSensorPinNames PinName { get; set; }

    public DistanceSensorPin(string name, int pinNumber, DistanceSensorPinNames pinName, PinMode mode)
    {
      Name = name;
      PinNumber = pinNumber;
      PinName = pinName;
      Mode = mode;
    }
  }
}
