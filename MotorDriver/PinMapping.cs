namespace MotorDriver
{
  public class PinMapping : IPinMapping
  {
    public int PinPwm { get; private set; }
    public int PinInput1 { get; private set; }
    public int PinInput2 { get; private set; }
    public int PinStandBy { get; private set; }

    public void CreatePinMapping(int pwm, int input1, int input2, int standBy)
    {
      PinPwm = pwm;
      PinInput1 = input1;
      PinInput2 = input2;
      PinStandBy = standBy;
    }
  }
}
