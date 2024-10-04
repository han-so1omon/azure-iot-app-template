using IoTAgentSimulator.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoTAgentSimulator.Services
{
    public class SimulatorService
    {
        private readonly DeviceService _deviceService;
        private readonly string _deviceId;

        public SimulatorService(DeviceService deviceService, string deviceId)
        {
            _deviceService = deviceService;
            _deviceId = deviceId;
        }

        public async Task StartSimulationAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Starting IoT device simulation for device {_deviceId}...");

            // Register the device with the IoT Device API
            var deviceState = new DeviceState
            {
                DeviceId = _deviceId,
                Name = "Simulated Temperature Sensor",
                Status = "active",
                Temperature = 0.0,  // Initial temperature
                IsOnline = true
            };

            var registrationSuccess = await _deviceService.AddDeviceAsync(deviceState);

            if (!registrationSuccess)
            {
                Console.WriteLine($"Failed to register device {_deviceId}. Simulation will not start for this device.");
                return;
            }

            Console.WriteLine($"Device {_deviceId} registered successfully. Starting simulation...");

            var random = new Random();

            while (!cancellationToken.IsCancellationRequested)
            {
                // Update the device state
                deviceState.Temperature = random.NextDouble() * 100;  // Random temperature between 0 and 100
                deviceState.Status = "active";

                // Update device state via the IoT Device API
                await _deviceService.UpdateDeviceStateAsync(deviceState);

                // Push the device state to the ingress API
                await _deviceService.PushDeviceStateAsync(_deviceId);

                // Wait for a random period between 3 to 5 seconds
                var delay = random.Next(3000, 5000);
                await Task.Delay(delay, cancellationToken);
            }

            Console.WriteLine($"Simulation for device {_deviceId} stopped.");
        }
    }
}
