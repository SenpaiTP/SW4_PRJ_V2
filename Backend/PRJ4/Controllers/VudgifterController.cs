using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using PRJ4.Data;
using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Services;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace PRJ4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VudgifterController : ControllerBase
    {
        private readonly IVudgifterService _VudgifterService;
        private readonly ILogger<VudgifterController> _logger;

        public VudgifterController(IVudgifterService VudgifterService, ILogger<VudgifterController> logger)
        {
            _VudgifterService = VudgifterService;
            _logger = logger;
        }

        private int GetUserId()
        {
            var brugerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(brugerIdClaim) || !int.TryParse(brugerIdClaim, out int brugerId))
            {
                _logger.LogError("Invalid or missing user ID claim. ClaimValue: {BrugerIdClaim}", brugerIdClaim);
                throw new UnauthorizedAccessException("Invalid or missing user ID claim.");
            }
            _logger.LogDebug("Retrieved user ID: {BrugerId}", brugerId);
            return brugerId;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VudgifterResponseDTO>>> GetAllByUser()
        {
            try
            {
                int brugerId = GetUserId();
                _logger.LogInformation("Fetching all fixed expenses for user {BrugerId}", brugerId);

                var Vudgifter = await _VudgifterService.GetAllByUser(brugerId);

                _logger.LogInformation("Successfully retrieved {Count} fixed expenses for user {BrugerId}", Vudgifter.Count(), brugerId);
                return Ok(Vudgifter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching fixed expenses for user");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<VudgifterResponseDTO>> Add(nyVudgifterDTO Vudgifter)
        {
            try
            {
                int brugerId = GetUserId();
                _logger.LogInformation("Attempting to add a new fixed expense for user {BrugerId}. Request: {@Vudgifter}", brugerId, Vudgifter);

                var response = await _VudgifterService.AddVudgifter(brugerId, Vudgifter);

                _logger.LogInformation("Successfully added new fixed expense with ID {VudgiftId} for user {BrugerId}", response.VudgiftId, brugerId);
                return CreatedAtAction(nameof(Add), new { id = response.VudgiftId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new fixed expense for user");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("opdater/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VudgifterUpdateDTO updateDTO)
        {
            try
            {
                int brugerId = GetUserId();
                _logger.LogInformation("Attempting to update fixed expense {Id} for user {BrugerId}. Update: {@UpdateDTO}", id, brugerId, updateDTO);

                await _VudgifterService.UpdateVudgifter(id, brugerId, updateDTO);

                _logger.LogInformation("Successfully updated fixed expense {Id} for user {BrugerId}", id, brugerId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating fixed expense {Id} for user", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}/slet")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int brugerId = GetUserId();
                _logger.LogInformation("Attempting to delete fixed expense {Id} for user {BrugerId}", id, brugerId);

                await _VudgifterService.DeleteVudgifter(brugerId, id);

                _logger.LogInformation("Successfully deleted fixed expense {Id} for user {BrugerId}", id, brugerId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting fixed expense {Id} for user", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }


}