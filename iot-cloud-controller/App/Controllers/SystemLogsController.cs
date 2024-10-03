using Microsoft.AspNetCore.Mvc;
using YourNamespace.Models;
using YourNamespace.Services;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemLogsController : ControllerBase
    {
        private readonly SystemLogsService _systemLogsService;

        public SystemLogsController(SystemLogsService systemLogsService) =>
            _systemLogsService = systemLogsService;

        [HttpGet]
        public async Task<List<SystemLog>> Get() =>
            await _systemLogsService.GetAsync();

        [HttpPost]
        public async Task<IActionResult> Create(SystemLog newSystemLog)
        {
            await _systemLogsService.CreateAsync(newSystemLog);
            return CreatedAtAction(nameof(Get), new { id = newSystemLog.Id }, newSystemLog);
        }
    }
}
