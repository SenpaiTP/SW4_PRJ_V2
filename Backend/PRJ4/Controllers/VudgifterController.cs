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

        public VudgifterController(IVudgifterService VudgifterService)
        {
            _VudgifterService = VudgifterService;
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
        public async Task<ActionResult<IEnumerable<VudgifterResponseDTO>>> GetAllByUser()
        {
            try
            {
                int brugerId = GetUserId();
                var Vudgifter = await _VudgifterService.GetAllByUser(brugerId);
                return Ok(Vudgifter);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<VudgifterResponseDTO>> Add(nyVudgifterDTO Vudgifter)
        {
            try
            {
                int brugerId = GetUserId();
                var response = await _VudgifterService.AddVudgifter(brugerId, Vudgifter);
                return CreatedAtAction(nameof(Add), new { id = response.VudgiftId }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("opdater/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VudgifterUpdateDTO updateDTO)
        {
            try
            {
                int brugerId = GetUserId();
                await _VudgifterService.UpdateVudgifter( id,brugerId, updateDTO);
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
                await _VudgifterService.DeleteVudgifter(brugerId, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}