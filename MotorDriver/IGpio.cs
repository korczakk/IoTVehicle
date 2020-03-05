using System.Device.Gpio;

namespace MotorDriver
{
  public interface IGpio
  {
    GpioController controller { get; }
  }
}