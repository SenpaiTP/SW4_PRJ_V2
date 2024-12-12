using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
//using PRJ4.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Data;
using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using FindtægtUpdateDTO = PRJ4.DTOs.FindtægtUpdateDTO;

using PRJ4.Services;

namespace PRJ4.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class IndstillingerController : ControllerBase
    {
        private readonly IIndstillingerRepo _indstillingerRepo;
        private readonly IIndstillingerService _indstillingerService;
        public IndstillingerController(IIndstillingerRepo indstillingerRepo, IIndstillingerService indstillingerService )
        {
            _indstillingerRepo = indstillingerRepo;
            _indstillingerService = indstillingerService;
        }

    [HttpGet("GetIndstillinger")]
    public async Task<ActionResult<IEnumerable<IndstillingerDTO>>> GetIndstillingerAsync()
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var Indstillinger = await _indstillingerRepo.GetIndstillingerAsync(userIdClaim.Value);
        return Ok(Indstillinger);

    }

    [HttpPut("UpdateIndstillinger/{id}")]
    public async Task<IActionResult> UpdateIndstillingerAsync(int id, [FromBody] IndstillingerDTO indstillingerDTO)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        
        var result = await _indstillingerService.UpdateIndstillingerAsync(userIdClaim.Value, id, indstillingerDTO);
        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }

     [HttpGet("GetTheme")]
    public async Task<ActionResult<IEnumerable<UpdateThemeDTO>>> GetThemeAsync()
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var Indstillinger = await _indstillingerRepo.GetThemeAsync( userIdClaim.Value);
        return Ok(Indstillinger);
    }

    [HttpPut("UpdateTheme/{id}")]
    public async Task<IActionResult> UpdateThemeAsync(int id, UpdateThemeDTO updateThemeDTO)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last() == "nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var result = await _indstillingerService.UpdateThemeAsync(userIdClaim.Value, id, updateThemeDTO);

        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost("AddIndstillinger")]
    public async Task<IActionResult> AddIndstillingerAsync( IndstillingerDTO indstillingerDTO)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }

        var indstillinger = await _indstillingerService.AddIndstillingerAsync(userIdClaim.Value, indstillingerDTO);
        return Ok(indstillinger);
    }

    [HttpPost("AddTheme")]
    public async Task<IActionResult> AddThemeAsync( UpdateThemeDTO updateThemeDTO)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }

        var indstillinger = await _indstillingerService.AddThemeAsync(userIdClaim.Value, updateThemeDTO);
        return Ok(indstillinger);
    }
    
    }


