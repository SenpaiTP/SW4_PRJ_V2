using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Services;
using System.Security.Claims;

namespace PRJ4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FudgifterController : ControllerBase
    {
        private readonly IFudgifterService _fudgifterService;
        private readonly ILogger<FudgifterController> _logger;

        public FudgifterController(IFudgifterService fudgifterService, ILogger<FudgifterController> logger)
        {
            _fudgifterService = fudgifterService;
            _logger = logger;
        }

        private int GetUserId()
        {
            var brugerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(brugerIdClaim) || !int.TryParse(brugerIdClaim, out int brugerId))
            {
                _logger.LogError("Invalid or missing user ID claim. {IdClaim}", brugerIdClaim);
                throw new UnauthorizedAccessException("Invalid or missing user ID claim.");
            }
            return brugerId;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FudgifterResponseDTO>>> GetAllByUser()
        {
            try
            {
                int brugerId = GetUserId();
                _logger.LogInformation("Fetching all fixed expenses for user {BrugerId}.", brugerId);

                var fudgifter = await _fudgifterService.GetAllByUser(brugerId);

                _logger.LogInformation("Successfully retrieved {Count} fixed expenses for user {BrugerId}.", fudgifter.Count(), brugerId);
                return Ok(fudgifter);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching fixed expenses for user: {Message}.", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<FudgifterResponseDTO>> Add(nyFudgifterDTO fudgifter)
        {
            try
            {
                int brugerId = GetUserId();
                _logger.LogInformation("Adding a new fixed expense for user {BrugerId}.", brugerId);

                var response = await _fudgifterService.AddFudgifter(brugerId, fudgifter);

                _logger.LogInformation("Successfully added fixed expense {FudgiftId} for user {BrugerId}.", response.FudgiftId, brugerId);
                return CreatedAtAction(nameof(Add), new { id = response.FudgiftId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding fixed expense: {Message}.", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("opdater/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FudgifterUpdateDTO updateDTO)
        {
            try
            {
                int brugerId = GetUserId();
                _logger.LogInformation("Updating fixed expense {Id} for user {BrugerId} with data: {@UpdateDTO}.", id, brugerId, updateDTO);

                await _fudgifterService.UpdateFudgifter(brugerId, id, updateDTO);

                _logger.LogInformation("Successfully updated fixed expense {Id} for user {BrugerId}.", id, brugerId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating fixed expense {Id}: {Message}.", id, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/slet")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int brugerId = GetUserId();
                _logger.LogInformation("Deleting fixed expense {Id} for user {BrugerId}.", id, brugerId);

                await _fudgifterService.DeleteFudgifter(brugerId, id);

                _logger.LogInformation("Successfully deleted fixed expense {Id} for user {BrugerId}.", id, brugerId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting fixed expense {Id}: {Message}.", id, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
