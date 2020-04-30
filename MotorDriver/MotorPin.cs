using IoT.Shared;
using System.Device.Gpio;

namespace MotorDriver
{
  public class MotorPin : IPin
  {
    public PinMode Mode { get; }

    public string Name { get; }

    public int PinNumber { get; }

    public MotorPinNames PinName { get; }

    public MotorPin(MotorPinNames name, int number, PinMode mode)
    {
      PinName = name;
      PinNumber = number;
      Mode = mode;
      Name = name.ToString();
    }
  }
}
