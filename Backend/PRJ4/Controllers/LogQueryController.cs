using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRJ4.DTOs;
using PRJ4.Models;
using PRJ4.Services;

namespace PRJ4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogQueryService _logQueryService;

        public LogController(ILogQueryService logQueryService)
        {
            _logQueryService = logQueryService;
        }

        // GET: api/logs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogDto>>> GetLogs()
        {
            var logs = await _logQueryService.GetAllLogsAsync();
            return Ok(logs);
        }

        // GET: api/logs/level/{level}
        [HttpGet("level/{level}")]
        public async Task<ActionResult<IEnumerable<LogDto>>> GetLogsByLevel(string level)
        {
            var logs = await _logQueryService.GetLogsByLevelAsync(level);
            return Ok(logs);
        }

        // GET: api/logs/date-range
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<LogDto>>> GetLogsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var logs = await _logQueryService.GetLogsByDateRangeAsync(startDate, endDate);
            return Ok(logs);
        }

        [HttpGet("request")]
        public async Task<ActionResult<IEnumerable<LogDto>>> GetLogsByRequest(string request)
        {
            var logs = await _logQueryService.GetLogsByRequest(request);
            return Ok(logs);
        }
        
    }

}