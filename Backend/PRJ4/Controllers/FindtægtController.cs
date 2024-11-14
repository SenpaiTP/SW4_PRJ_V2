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
    public async Task<ActionResult<IEnumerable<Findtægt>>> GetFindtægt()
    {
        var findtægt = await _findtægtRepo.GetAllAsync();
        return Ok(findtægt);
    }

    [HttpPost]
    public async Task<IActionResult> AddFindtægt(FindtægtDTO findtægtDto)
    {
        var findtægt = new Findtægt
        {
            BrugerId = findtægtDto.BrugerId,
            Tekst = findtægtDto.Tekst,
            Indtægt = findtægtDto.Indtægt,
            Dato = findtægtDto.Dato
        };
        await _findtægtservice.AddAsync(findtægt);
        return CreatedAtAction(nameof(GetFindtægt), new { id = findtægt.FindtægtId }, findtægt);
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