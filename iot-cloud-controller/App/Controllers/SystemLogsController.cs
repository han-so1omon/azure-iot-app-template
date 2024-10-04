using Microsoft.AspNetCore.Mvc;
using YourNamespace.Models;
using YourNamespace.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemLogsController : ControllerBase
    {
        private readonly SystemLogsService _systemLogsService;

        public SystemLogsController(SystemLogsService systemLogsService) =>
            _systemLogsService = systemLogsService;

        // Get a specific log by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<SystemLog>> Get(string id)
        {
            var log = await _systemLogsService.GetAsync(id);

            if (log is null)
            {
                return NotFound();
            }

            return Ok(log);
        }

        // Get recent logs (past 10 minutes, max 100 logs)
        [HttpGet("recent")]
        public async Task<ActionResult<List<SystemLog>>> GetRecentLogs()
        {
            var logs = await _systemLogsService.GetRecentLogsAsync();
            return Ok(logs);
        }

        // New route to get the count of logs with types INFO, WARNING, or ERROR
        [HttpGet("log-count")]
        public async Task<ActionResult<Dictionary<string, int>>> GetLogCountByType()
        {
            var logCountByType = await _systemLogsService.GetLogCountByTypeAsync();
            return Ok(logCountByType);
        }

         // Create a new log
        [HttpPost]
        public async Task<IActionResult> Create(SystemLog newSystemLog)
        {
            await _systemLogsService.CreateAsync(newSystemLog);
            return CreatedAtAction(nameof(Get), new { id = newSystemLog.Id }, newSystemLog);
        }
    }
}
