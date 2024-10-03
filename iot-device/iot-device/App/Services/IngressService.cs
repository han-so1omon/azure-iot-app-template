using IoTDeviceApp.Models;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoTDeviceApp.Services
{
    public class MqttIngressService
    {
        private readonly IMqttClient _mqttClient;
        private readonly string _clientId;

        public MqttIngressService()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            // Get the container name from the HOSTNAME environment variable
            _clientId = Environment.GetEnvironmentVariable("HOSTNAME") ?? "IoTDeviceClient";
        }

        public async Task ConnectAsync()
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId(_clientId) // Use container name as client ID
                .WithTcpServer("iot-ingress-service", 1883)
                .WithCleanSession()
                .Build();

            await _mqttClient.ConnectAsync(options, CancellationToken.None);
            Console.WriteLine($"Connected to MQTT broker with Client ID: {_clientId}");
            await PushSystemLogAsync(new SystemLog { LogType = "Info", Message = "Connected to MQTT broker", Timestamp = DateTime.UtcNow });
        }

        public async Task PushDeviceStateAsync(DeviceState deviceState)
        {
            var json = JsonSerializer.Serialize(deviceState);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("iot/temperature-readings")
                .WithPayload(json)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();

            await _mqttClient.PublishAsync(message, CancellationToken.None);
            await PushSystemLogAsync(new SystemLog { LogType = "Info", Message = $"Temperature reading sent via MQTT from Client ID: {_clientId}", Timestamp = DateTime.UtcNow });
            Console.WriteLine($"Temperature reading sent via MQTT from Client ID: {_clientId}");
        }

        public async Task PushSystemLogAsync(SystemLog log)
        {
            var json = JsonSerializer.Serialize(log);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("iot/system-logs")
                .WithPayload(json)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();

            await _mqttClient.PublishAsync(message, CancellationToken.None);
            Console.WriteLine($"System log sent via MQTT from Client ID: {_clientId}");
        }

        public async Task DisconnectAsync()
        {
            await PushSystemLogAsync(new SystemLog { LogType = "Info", Message = "Disconnecting from MQTT broker", Timestamp = DateTime.UtcNow });
            await _mqttClient.DisconnectAsync();
            Console.WriteLine($"Disconnected from MQTT broker (Client ID: {_clientId})");
        }
    }
}
