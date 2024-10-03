using IoTAgentSimulator.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration settings
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("/App/appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var devices = configuration.GetSection("Devices").Get<List<DeviceConfig>>();

        if (devices == null || devices.Count == 0)
        {
            Console.WriteLine("No devices are configured in appsettings.json");
            return;
        }

        using var cts = new CancellationTokenSource();

        var simulationTasks = new List<Task>();

        // Start a simulation task for each device
        foreach (var device in devices)
        {
            var deviceService = new DeviceService(device.DeviceApiBaseUrl);
            var simulatorService = new SimulatorService(deviceService, device.DeviceId);

            var simulationTask = simulatorService.StartSimulationAsync(cts.Token);
            simulationTasks.Add(simulationTask);

            Console.WriteLine($"Started simulation for device {device.DeviceId}");
        }

        Console.WriteLine("IoT device simulations started. Running indefinitely until container is stopped...");

        // Wait indefinitely until the cancellation token is triggered (e.g., Docker container stops)
        try
        {
            await Task.Delay(Timeout.Infinite, cts.Token);
        }
        catch (TaskCanceledException)
        {
            // This exception is expected when the cancellation token is triggered
            Console.WriteLine("Cancellation requested. Stopping simulations...");
        }

        // Stop all simulations
        cts.Cancel();
        await Task.WhenAll(simulationTasks);

        Console.WriteLine("IoT device simulations completed.");
    }
}

// Class to map the device configuration from appsettings.json
public class DeviceConfig
{
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceApiBaseUrl { get; set; } = string.Empty;
}
