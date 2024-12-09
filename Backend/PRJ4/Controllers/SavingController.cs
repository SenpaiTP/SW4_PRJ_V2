using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;
using PRJ4.Services;
using Microsoft.AspNetCore.Authorization;

namespace PRJ4.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SavingController : ControllerBase
    {
        private readonly ISavingService _savingService;
        private readonly ILogger<SavingController> _logger;

        public SavingController(ISavingService savingService, ILogger<SavingController> logger)
        {
            _savingService = savingService;
            _logger = logger;
        }

        
        // Method to extract user ID from claims
        private string GetUserId() 
        {
            var claims = Request.HttpContext.User.Claims;
            var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last() == "nameidentifier");
            if (userIdClaim == null)
            {
                return null;
            }
            return userIdClaim.Value;
        }
        


        // Get all savings for a specific budget
        [HttpGet("{budgetId}")]
        public async Task<IActionResult> GetAllSavingsAsync(int budgetId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                _logger.LogWarning("User ID is missing.");
                return Unauthorized(new { Message = "User not authenticated" });
            }
            try
            {
                var result = await _savingService.GetAllSavingsAsync(budgetId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving savings.");
                return NotFound(new { message = ex.Message });
            }
        }

        // Get a specific saving by its ID
        [HttpGet("/saving/{savingId}")]
        public async Task<IActionResult> GetSavingByIdAsync(int savingId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                _logger.LogWarning("User ID is missing.");
                return Unauthorized(new { Message = "User not authenticated" });
            }
            try
            {
                var result = await _savingService.GetSavingByIdAsync(savingId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving saving.");
                return NotFound(new { message = ex.Message });
            }
        }
        // Add a new saving
        [HttpPost("{budgetId}")]
        public async Task<IActionResult> AddSavingAsync(int budgetId, [FromBody] SavingCreateDTO savingDTO)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                _logger.LogWarning("User ID is missing.");
                return Unauthorized(new { Message = "User not authenticated" });
            }
            try
            {
                var result = await _savingService.AddSavingAsync(budgetId, userId, savingDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding saving.");
                return BadRequest(new { message = ex.Message });
            }
        }

        // Update an existing saving
        [HttpPut("{budgetId}/{savingId}")]
        public async Task<IActionResult> UpdateSavingAsync(int savingId, int budgetId, [FromBody] SavingUpdateDTO savingUpdateDTO)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                _logger.LogWarning("User ID is missing.");
                return Unauthorized(new { Message = "User not authenticated" });
            }
            try
            {
                var result = await _savingService.UpdateSavingAsync(savingId, userId, savingUpdateDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating saving.");
                return BadRequest(new { message = ex.Message });
            }
        }

        // Delete a saving
        [HttpDelete("{budgetId}/{savingId}")]
        public async Task<IActionResult> DeleteSavingAsync(int savingId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                _logger.LogWarning("User ID is missing.");
                return Unauthorized(new { Message = "User not authenticated" });
            }
            try
            {
                await _savingService.DeleteSavingAsync(savingId, userId);
                return NoContent();  // No content to return as the saving is deleted
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting saving.");
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
