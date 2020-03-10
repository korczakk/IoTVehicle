using System.Threading.Tasks;

namespace IoTVehicle.Api.Services
{
    public interface IDriveService
    {
        void GoBackward();
        void GoForward();
        void StopDrive();
        Task TurnLeft();
        Task TurnRight();
    }
}