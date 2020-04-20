namespace IoT.Shared
{
  public interface IPinMapping
  {
    int PinInput1 { get; }
    int PinInput2 { get; }
    int PinPwm { get; }
    int PinStandBy { get; }

    void CreatePinMapping(int pwm, int input1, int input2, int standBy);
  }
}