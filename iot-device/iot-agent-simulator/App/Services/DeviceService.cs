using IoTAgentSimulator.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoTAgentSimulator.Services
{
    public class DeviceService
    {
        private readonly HttpClient _httpClient;
        private readonly string _deviceApiBaseUrl;

        public DeviceService(string deviceApiBaseUrl)
        {
            _httpClient = new HttpClient();
            _deviceApiBaseUrl = deviceApiBaseUrl;
        }

        // Method to add/register a device to the IoT Device API
        public async Task<bool> AddDeviceAsync(DeviceState deviceState)
        {
            try
            {
                var json = JsonSerializer.Serialize(deviceState);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_deviceApiBaseUrl}/api/device", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Successfully registered device: {deviceState.DeviceId}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to register device: {deviceState.DeviceId}. Error: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while registering device: {ex.Message}");
                return false;
            }
        }

        public async Task UpdateDeviceStateAsync(DeviceState deviceState)
        {
            var json = JsonSerializer.Serialize(deviceState);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_deviceApiBaseUrl}/api/device/{deviceState.DeviceId}", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully updated state for device: {deviceState.DeviceId}");
            }
            else
            {
                Console.WriteLine($"Failed to update state for device: {deviceState.DeviceId}. Error: {response.ReasonPhrase}");
            }
        }

        public async Task PushDeviceStateAsync(string deviceId)
        {
            var response = await _httpClient.PostAsync($"{_deviceApiBaseUrl}/api/device/{deviceId}/push-state", null);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Device state for {deviceId} pushed to the ingress API.");
            }
            else
            {
                Console.WriteLine($"Failed to push device state for {deviceId}. Error: {response.ReasonPhrase}");
            }
        }
    }
}
