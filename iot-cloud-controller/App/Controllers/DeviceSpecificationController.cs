using Microsoft.AspNetCore.Mvc;
using YourNamespace.Models;
using YourNamespace.Services;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceSpecificationsController : ControllerBase
    {
        private readonly DeviceSpecificationsService _deviceSpecificationsService;

        public DeviceSpecificationsController(DeviceSpecificationsService deviceSpecificationsService) =>
            _deviceSpecificationsService = deviceSpecificationsService;

        [HttpGet]
        public async Task<List<DeviceSpecification>> Get() =>
            await _deviceSpecificationsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<DeviceSpecification>> Get(string id)
        {
            var deviceSpecification = await _deviceSpecificationsService.GetAsync(id);

            if (deviceSpecification is null)
            {
                return NotFound();
            }

            return deviceSpecification;
        }

        [HttpPost]
        public async Task<IActionResult> Create(DeviceSpecification newDeviceSpecification)
        {
            await _deviceSpecificationsService.CreateAsync(newDeviceSpecification);
            return CreatedAtAction(nameof(Get), new { id = newDeviceSpecification.Id }, newDeviceSpecification);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, DeviceSpecification updatedDeviceSpecification)
        {
            var deviceSpecification = await _deviceSpecificationsService.GetAsync(id);

            if (deviceSpecification is null)
            {
                return NotFound();
            }

            updatedDeviceSpecification.Id = deviceSpecification.Id;

            await _deviceSpecificationsService.UpdateAsync(id, updatedDeviceSpecification);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deviceSpecification = await _deviceSpecificationsService.GetAsync(id);

            if (deviceSpecification is null)
            {
                return NotFound();
            }

            await _deviceSpecificationsService.RemoveAsync(id);

            return NoContent();
        }
    }
}
