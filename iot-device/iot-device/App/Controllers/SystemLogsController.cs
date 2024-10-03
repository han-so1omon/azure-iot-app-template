using IoTDeviceApp.Models;
using IoTDeviceApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IoTDeviceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemLogsController : ControllerBase
    {
        private readonly MqttIngressService _ingressService;

        public SystemLogsController(MqttIngressService ingressService)
        {
            _ingressService = ingressService;
        }

        [HttpPost]
        public async Task<IActionResult> LogSystemMessage([FromBody] SystemLog log)
        {
            await _ingressService.PushSystemLogAsync(log);
            return Ok("System log pushed via MQTT.");
        }
    }
}
