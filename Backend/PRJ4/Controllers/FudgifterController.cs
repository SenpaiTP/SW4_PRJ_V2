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

        private string GetUserId()
        {
            var claims = Request.HttpContext.User.Claims;
            var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last() == "nameidentifier");
            if(userIdClaim.Value != null)
            {
                return userIdClaim.Value;
            }
            return null;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FudgifterResponseDTO>>> GetAllByUser()
        {
            try
            {
                string brugerId = GetUserId();
                _logger.LogInformation("Fetching all fixed expenses for user {BrugerId}. {Method}", brugerId, HttpContext.Request.Method);

                var fudgifter = await _fudgifterService.GetAllByUser(brugerId);

                _logger.LogInformation("Successfully retrieved {Count} fixed expenses for user {BrugerId}. {Method}", fudgifter.Count(), brugerId, HttpContext.Request.Method);
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
                string brugerId = GetUserId();
                _logger.LogInformation("Adding a new fixed expense for user {BrugerId}. {Method}", brugerId, HttpContext.Request.Method);

                var response = await _fudgifterService.AddFudgifter(brugerId, fudgifter);

                _logger.LogInformation("Successfully added fixed expense {FudgiftId} for user {BrugerId}. {Method}", response.FudgiftId, brugerId, HttpContext.Request.Method);
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
                string brugerId = GetUserId();
                _logger.LogInformation("Updating fixed expense {Id} for user {BrugerId} with data: {@UpdateDTO}. {Method}", id, brugerId, updateDTO, HttpContext.Request.Method);

                await _fudgifterService.UpdateFudgifter(brugerId, id, updateDTO);

                _logger.LogInformation("Successfully updated fixed expense {Id} for user {BrugerId}. {Method}", id, brugerId, HttpContext.Request.Method);
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
                string brugerId = GetUserId();
                _logger.LogInformation("Deleting fixed expense {Id} for user {BrugerId}. {Method}", id, brugerId, HttpContext.Request.Method);

                await _fudgifterService.DeleteFudgifter(brugerId, id);

                _logger.LogInformation("Successfully deleted fixed expense {Id} for user {BrugerId}. {Method}", id, brugerId, HttpContext.Request.Method);
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
