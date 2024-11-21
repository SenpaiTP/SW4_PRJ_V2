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
    public class FudgifterController : ControllerBase
    {
        private readonly IFudgifterService _fudgifterService;

        public FudgifterController(IFudgifterService fudgifterService)
        {
            _fudgifterService = fudgifterService;
        }

        private int GetUserId()
        {
            var brugerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(brugerIdClaim) || !int.TryParse(brugerIdClaim, out int brugerId))
            {
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
                var fudgifter = await _fudgifterService.GetAllByUser(brugerId);
                return Ok(fudgifter);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<FudgifterResponseDTO>> Add(nyFudgifterDTO fudgifter)
        {
            try
            {
                int brugerId = GetUserId();
                var response = await _fudgifterService.AddFudgifter(brugerId, fudgifter);
                return CreatedAtAction(nameof(Add), new { id = response.FudgiftId }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("opdater/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FudgifterUpdateDTO updateDTO)
        {
            try
            {
                int brugerId = GetUserId();
                await _fudgifterService.UpdateFudgifter(brugerId, id, updateDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/slet")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int brugerId = GetUserId();
                await _fudgifterService.DeleteFudgifter(brugerId, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}