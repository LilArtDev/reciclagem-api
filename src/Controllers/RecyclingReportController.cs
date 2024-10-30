using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReciclagemApi.Models;
using ReciclagemApi.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ReciclagemApi.Controllers
{
    [Route("api/report")]
    [ApiController]
    [Authorize]
    public class RecyclingReportController : ControllerBase
    {
        private readonly IRecyclingReportService _reportService;

        public RecyclingReportController(IRecyclingReportService reportService)
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecyclingReportModel>>> GetUserReports([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim);
            var reports = await _reportService.GetUserReportsAsync(userId, page, pageSize);
            return Ok(reports);
        }

        [HttpPost]
        public async Task<IActionResult> AddReport([FromBody] RecyclingReportModel report)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim);
            await _reportService.AddReportAsync(userId, report.Material, report.Quantity);
            return CreatedAtAction(nameof(GetUserReports), new { id = report.ReportId }, report);
        }
    }
}
