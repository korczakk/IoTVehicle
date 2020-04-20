using IoT.Shared;

namespace MotorDriver
{
    public interface IMotor
    {
        void Initialize(IPinMapping pinMapping);

        void StartClockWise();

        void StartCounterClockWise();

        void Stop();
    }
}