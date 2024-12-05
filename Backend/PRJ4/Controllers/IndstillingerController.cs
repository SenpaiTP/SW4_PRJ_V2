using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using PRJ4.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Data;
using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using FindtægtUpdateDTO = PRJ4.DTOs.FindtægtUpdateDTO;
using PRJ4.Infrastructure;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndstillingerDTO>>> GetAllAsync()
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var Indstillinger = await _indstillingerRepo.GetAllAsync();
        return Ok(Indstillinger);

    }

    [HttpPut]
    public async Task<IActionResult> UpdateIndstillingerAsync(IndstillingerDTO indstillingerDTO)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var result = await _indstillingerService.UpdateIndstillingerAsync(indstillingerDTO);
        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPut("UpdateTheme")]
    public async Task<IActionResult> UpdateThemeAsync(UpdateThemeDTO updateThemeDTO)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last() == "nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }

        bool themeUpdated = updateThemeDTO.SetTheme;
         return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> AddIndstillingerAsync(IndstillingerDTO indstillingerDTO)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }

        var indstillinger = await _indstillingerService.AddIndstillingerAsync(indstillingerDTO);
        return Ok(indstillinger);
    }
    
    }


