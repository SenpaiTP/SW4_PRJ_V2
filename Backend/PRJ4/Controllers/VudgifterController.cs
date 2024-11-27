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
        public async Task<ActionResult<IEnumerable<VudgifterResponseDTO>>> GetAllByUser()
        {

             string brugerId = GetUserId();
            try
            {
                //int brugerId = GetUserId();
                _logger.LogInformation("Fetching all variable expenses for user with ID: {BrugerId} {Method}", brugerId, HttpContext.Request.Method);
                var response = await _VudgifterService.GetAllByUser(brugerId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching variable expenses for user");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<VudgifterResponseDTO>> AddVudgifter(nyVudgifterDTO dto)
        {
            try
            {
                string brugerId = GetUserId();
                _logger.LogInformation("Posting variable expense for user with ID {BrugerId}. {Method}", brugerId, HttpContext.Request.Method);
                var response = await _VudgifterService.AddVudgifter(brugerId, dto);
                return CreatedAtAction(nameof(GetAllByUser), new { id = response.VudgiftId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while adding variable expense for user {ex}", ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVudgifter(int id, VudgifterUpdateDTO updateDTO)
        {
            try
            {
                string brugerId = GetUserId();
                _logger.LogInformation("Updating  variable expense for user with ID {BrugerId}. {Method}", brugerId, HttpContext.Request.Method);
                // If the updateDTO is empty, return a BadRequest response with a log
                if (updateDTO == null || 
                    (!updateDTO.Pris.HasValue && string.IsNullOrWhiteSpace(updateDTO.Tekst) && !updateDTO.Dato.HasValue && !updateDTO.KategoriId.HasValue && string.IsNullOrWhiteSpace(updateDTO.KategoriNavn)))
                {
                    _logger.LogWarning("Update request for variable expense ID {VudgiftId} is empty or invalid.", id);
                    return BadRequest("No valid data provided for update.");
                }

                await _VudgifterService.UpdateVudgifter( brugerId,id, updateDTO);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid data provided for variable expense update.");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "variable expense not found for update.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating variable expense");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVudgifter(int id)
        {
            try
            {
                string brugerId = GetUserId();
                _logger.LogInformation("Trying to delete variable expense {id} on UserId{brugerId}. {Method}", id, brugerId,HttpContext.Request.Method);
                await _VudgifterService.DeleteVudgifter(brugerId, id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "variable expense not found for deletion.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting variable expense");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
