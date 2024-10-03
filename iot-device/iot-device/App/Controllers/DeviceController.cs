using IoTDeviceApp.Models;
using IoTDeviceApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IoTDeviceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceStateService _deviceStateService;
        private readonly MqttIngressService _mqttIngressService;

        public DeviceController(DeviceStateService deviceStateService, MqttIngressService mqttIngressService)
        {
            _deviceStateService = deviceStateService;
            _mqttIngressService = mqttIngressService;
        }

        [HttpGet]
        public IActionResult GetDevices() => Ok(_deviceStateService.GetAllDevices());

        [HttpGet("{deviceId}")]
        public IActionResult GetDevice(string deviceId)
        {
            var device = _deviceStateService.GetDevice(deviceId);
            if (device == null) return NotFound();
            return Ok(device);
        }

        [HttpPost]
        public IActionResult AddDevice([FromBody] DeviceState newDevice)
        {
            _deviceStateService.AddDevice(newDevice);
            return CreatedAtAction(nameof(GetDevice), new { deviceId = newDevice.DeviceId }, newDevice);
        }

        [HttpPut("{deviceId}")]
        public IActionResult UpdateDevice(string deviceId, [FromBody] DeviceState updatedState)
        {
            var existingDevice = _deviceStateService.GetDevice(deviceId);
            if (existingDevice == null) return NotFound();

            _deviceStateService.UpdateDeviceState(deviceId, updatedState);
            return NoContent();
        }

        [HttpDelete("{deviceId}")]
        public IActionResult DeleteDevice(string deviceId)
        {
            _deviceStateService.DeleteDevice(deviceId);
            return NoContent();
        }

        [HttpPost("{deviceId}/push-state")]
        public async Task<IActionResult> PushDeviceState(string deviceId)
        {
            var device = _deviceStateService.GetDevice(deviceId);
            if (device == null) return NotFound();

            await _mqttIngressService.PushDeviceStateAsync(device);
            return Ok("Device state pushed via MQTT.");
        }
    }
}
