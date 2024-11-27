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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out var userIdInt))
        {
            return BadRequest("Invalid user ID");
        }
        var findtægter = await _findtægtRepo.GetFindtægterByUserIdAsync(userIdInt);
        return Ok(findtægter);
    }

    [HttpPost]
    public async Task<ActionResult<FindtægtDTO>> CreateFindtægt(FindtægtCreateDTO findtægtCreateDTO)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out var userIdInt))
        {
            return BadRequest("Invalid user ID");
        }
        var findtægt = await _findtægtservice.CreateFindtægtAsync(userIdInt, findtægtCreateDTO);
        return CreatedAtAction(nameof(GetFindtægter), new { id = findtægt.FindtægtId }, findtægt);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFindtægt(int id, FindtægtUpdateDTO findtægtUpdateDTO)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out var userIdInt))
        {
            return BadRequest("Invalid user ID");
        }
        var result = await _findtægtservice.UpdateFindtægtAsync(userIdInt, id, findtægtUpdateDTO);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFindtægt(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out var userIdInt))
        {
            return BadRequest("Invalid user ID");
        }
        var result = await _findtægtservice.DeleteFindtægtAsync(userIdInt, id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}