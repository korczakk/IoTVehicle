namespace MotorDriver
{
  public interface IMotor
  {
    void Initialize(IPinMapping pinMapping);
    void StartMotor(RotateDirection direction);
    void StopMotor();
  }
}