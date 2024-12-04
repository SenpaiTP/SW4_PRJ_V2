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
using VindtægtUpdateDTO = PRJ4.DTOs.VindtægtUpdateDTO;
using PRJ4.Infrastructure;
using PRJ4.Services;

namespace PRJ4.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VindtægtController:ControllerBase
{
    private readonly IVindtægtRepo _vindtægtRepo;
    private readonly IVindtægtService _vindtægtService;

    public VindtægtController(IVindtægtRepo VindtægtRepo, IVindtægtService VindtægtService)
    {
        _vindtægtRepo = VindtægtRepo;
        _vindtægtService = VindtægtService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VindtægtDTO>>> GetVindtægter()
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var Vindtægter = await _vindtægtRepo.GetVindtægterByUserIdAsync(userIdClaim.Value);
        return Ok(Vindtægter);
    }

    [HttpPost]
    public async Task<ActionResult<VindtægtDTO>> CreateVindtægt(VindtægtCreateDTO vindtægtCreateDto)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        if(string.IsNullOrEmpty(vindtægtCreateDto.Tekst))
        {
            return BadRequest("Tekst is required");
        }
        var Vindtægt = await _vindtægtService.CreateVindtægtAsync(userIdClaim.Value, vindtægtCreateDto);
        return CreatedAtAction(nameof(GetVindtægter), new { id = Vindtægt.VindtægtId }, Vindtægt);
    }

    [HttpPut]
    
    public async Task<IActionResult> UpdateVindtægt([FromQuery] int findid, VindtægtUpdateDTO vindtægtUpdateDto)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var result = await _vindtægtService.UpdateVindtægtAsync(userIdClaim.Value, findid, vindtægtUpdateDto);
        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteVindtægt([FromQuery]int id)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var result = await _vindtægtService.DeleteVindtægtAsync(userIdClaim.Value, id);
        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }
}