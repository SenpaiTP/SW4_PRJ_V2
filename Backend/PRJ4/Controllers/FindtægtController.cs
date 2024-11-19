using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using PRJ4.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Data;
using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using PRJ4.Infrastructure;
using PRJ4.Services;

namespace PRJ4.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FindtægtController:ControllerBase
{
    private readonly IFindtægtRepo _findtægtRepo;
    private readonly TokenProvider _tokenProvider;
    private readonly IFindtægtService _findtægtservice;

    public FindtægtController(IFindtægtRepo findtægtRepo, TokenProvider tokenProvider, IFindtægtService findtægtService)
    {
        _findtægtRepo = findtægtRepo;
        _tokenProvider = tokenProvider;
        _findtægtservice = findtægtService;
    }


    [HttpGet]
    [Authorize]

    public async Task<ActionResult<IEnumerable<Findtægt>>> GetFindtægt()
    {
        var findtægt = await _findtægtRepo.GetAllAsync();
        return Ok(findtægt);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddFindtægtAsync(FindtægtDTO findtægtDto)
    {
        if (findtægtDto == null)
        {
            return BadRequest("Fast Indtægt kan ikke være 0");
        }
        var findtægt = new Findtægt
        {
            Tekst = findtægtDto.Tekst,
            Indtægt = findtægtDto.Indtægt,
            Dato = findtægtDto.Dato
        };

        var user = User;
        await _findtægtservice.AddFindtægtAsync(findtægt,user);
        await _findtægtRepo.SaveChangesAsync();
        return Ok(findtægt);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteFindtægt(int id)
    {
        var findtægt = await _findtægtRepo.Delete(id);
        if (findtægt == null)
        {
            return NotFound();
        }
        return NoContent();
    }
}