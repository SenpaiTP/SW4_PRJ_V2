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
public class FindtægtController:ControllerBase
{
    private readonly IFindtægtRepo _findtægtRepo;
    private readonly IFindtægtService _findtægtservice;

    public FindtægtController(IFindtægtRepo findtægtRepo, IFindtægtService findtægtService)
    {
        _findtægtRepo = findtægtRepo;
        _findtægtservice = findtægtService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FindtægtDTO>>> GetFindtægter()
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var findtægter = await _findtægtRepo.GetFindtægterByUserIdAsync(userIdClaim.Value);
        return Ok(findtægter);
    }

    [HttpGet, Route("id")]
    public async Task<ActionResult<FindtægtDTO>> GetFindtægtId([FromQuery] int id)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var findtægt = await _findtægtRepo.GetById(userIdClaim.Value, id);
        if (findtægt == null)
        {
            return NotFound();
        }
        return Ok(findtægt);
    }

    [HttpPost]
    public async Task<ActionResult<FindtægtDTO>> CreateFindtægt(FindtægtCreateDTO findtægtCreateDTO)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        if(string.IsNullOrEmpty(findtægtCreateDTO.Tekst))
        {
            return BadRequest("Tekst is required");
        }
        var findtægt = await _findtægtservice.CreateFindtægtAsync(userIdClaim.Value, findtægtCreateDTO);
        return CreatedAtAction(nameof(GetFindtægter), new { id = findtægt.FindtægtId }, findtægt);
    }

    [HttpPut]
    
    public async Task<IActionResult> UpdateFindtægt([FromQuery] int findid, FindtægtUpdateDTO findtægtUpdateDTO)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var result = await _findtægtservice.UpdateFindtægtAsync(userIdClaim.Value, findid, findtægtUpdateDTO);
        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFindtægt([FromQuery]int id)
    {
        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");
        if (userIdClaim == null)
        {
            return BadRequest("Invalid user ID");
        }
        var result = await _findtægtservice.DeleteFindtægtAsync(userIdClaim.Value, id);
        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }
}