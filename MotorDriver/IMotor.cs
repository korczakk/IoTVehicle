using IoT.Shared;
using System.Collections.Generic;

namespace MotorDriver
{
  public interface IMotor
  {
    void Initialize(IEnumerable<MotorPin> pinMapping);

    void StartClockWise();

    void StartCounterClockWise();

    void Stop();

    MotorState GetMotorState();
  }
}