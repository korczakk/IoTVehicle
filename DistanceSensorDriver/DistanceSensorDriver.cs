using IoT.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DistanceSensor
{
  public class DistanceSensorDriver : IDistanceSensorDriver
  {
    private readonly IGpio gpio;
    private readonly ILogger logger;
    private Dictionary<DistanceSensorPinNames, DistanceSensorPin> pinMapping;
    private Stopwatch programRuntime;

    public DistanceSensorDriver(IGpio gpio, ILogger logger, IEnumerable<DistanceSensorPin> pinMapping)
    {
      this.gpio = gpio;
      this.logger = logger;
      this.pinMapping = pinMapping.ToDictionary(pin => pin.PinName);
    }

    public async Task<double> MeasureDistance()
    {
      var echoDuration = new Stopwatch();
      var waitingForEcho = new Stopwatch();

      // trigger
      gpio.SetPin(pinMapping[DistanceSensorPinNames.Trigger].PinNumber, PinValue.High);
      await Task.Delay(TimeSpan.FromTicks(100));
      gpio.SetPin(pinMapping[DistanceSensorPinNames.Trigger].PinNumber, PinValue.Low);

      // Wait for High state on echo pin
      waitingForEcho.Reset();
      waitingForEcho.Start();
      while (gpio.ReadPin(pinMapping[DistanceSensorPinNames.Echo].PinNumber) == PinValue.Low && waitingForEcho.Elapsed.TotalMilliseconds <= 100) { }
      waitingForEcho.Stop();
      // logger.LogInformation($"Waiting for High value on Echo pin: {waitingForEcho.Elapsed.Milliseconds}");

      // Measure duration of high state on echo pin
      echoDuration.Reset();
      echoDuration.Start();
      while (gpio.ReadPin(pinMapping[DistanceSensorPinNames.Echo].PinNumber) == PinValue.High) { }
      echoDuration.Stop();
      var duration = echoDuration.Elapsed.Ticks / 10;
      // logger.LogInformation($"High value duration: {duration} microseconds");

      var distance = (duration * 34.0) / 1000 / 2;
      logger.LogInformation($"Measured distance: {distance} ({Math.Round(distance)})");

      return await Task.FromResult(distance);
    }
  }
}
