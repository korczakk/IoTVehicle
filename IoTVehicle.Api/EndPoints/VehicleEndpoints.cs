using IoTVehicle.Api.Services;
using Microsoft.AspNetCore.Http;
using MotorDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTVehicle.Api.EndPoints
{
    public class VehicleEndpoints
    {
        public static async Task GoForward(HttpContext context)
        {
            var services = context.RequestServices;
            var driveService = services.GetService(typeof(IDriveService));

            // TO DO

            await context.Response.WriteAsync("Moving forward");
        }

        public static async Task Index(HttpContext context)
        {
            await context.Response.WriteAsync("Possible endpoints: GoForward, GoBackWard, TurnLeft, TurnRight");
        }
    }
}
