using System.Threading.Tasks;

namespace IoTVehicle.Api.Services
{
  public interface IDriveService
  {
    void GoBackward();
    Task GoBackward(int moveTime);
    void GoForward();
    Task GoForward(int moveTime);
    void StopDrive();
    Task TurnLeft();
    Task TurnLeft(int moveTime);
    Task TurnRight();
    Task TurnRight(int moveTime);
    bool IsGoingForward();
  }
}