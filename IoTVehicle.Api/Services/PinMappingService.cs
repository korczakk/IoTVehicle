using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MotorDriver;
using System;
using System.Collections.Generic;

namespace IoTVehicle.Api.Services
{
  public class PinMappingService : IPinMappingService
  {
    private readonly ILogger logger;
    private readonly IConfiguration configuration;
    private IDictionary<int, IPinMapping> pinMappings;

    public PinMappingService(ILogger<PinMappingService> logger, IConfiguration configuration)
    {
      this.logger = logger;
      this.configuration = configuration;

      pinMappings = new Dictionary<int, IPinMapping>();
    }

    public void CreateMappings()
    {
      var mappingForMotor1 = configuration
        .GetSection("PinMappings:1")
        .Get<PinMapping>(x => x.BindNonPublicProperties = true);

      var mappingForMotor2 = configuration
        .GetSection("PinMappings:0")
        .Get<PinMapping>(x => x.BindNonPublicProperties = true);

      if(mappingForMotor1 is null || mappingForMotor2 is null)
      {
        logger.LogError("Configuration for motors not present.");
        throw new ArgumentNullException();
      }

      pinMappings.Add(1, mappingForMotor1);
      pinMappings.Add(2, mappingForMotor2);
    }

    public IEnumerable<IPinMapping> GetAllMappings()
    {
      return pinMappings.Values;
    }

    public IPinMapping GetMapping(int motorNumber)
    {
      if (!pinMappings.ContainsKey(motorNumber))
      {
        logger.LogError("Motor index is out of range");
        throw new ArgumentOutOfRangeException("Motor number out of range");
      }

      return pinMappings[motorNumber];
    }
  }
}
