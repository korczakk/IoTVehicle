using System.Threading.Tasks;

namespace IoTVehicle.Api.Services
{
  public interface IDriveService
  {
    void GoBackward();
    void GoBackwardLeft();
    void GoBackwardRight();
    void GoForward();
    void GoForwardLeft();
    void GoForwardRight();
    void StopDrive();
    void StopRightDrive();
    void StopLeftDrive();
    Task TurnLeft();
    Task TurnRight();
    bool IsGoingForward();
  }
}