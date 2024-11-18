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


namespace PRJ4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class BrugerController:ControllerBase
    {
        private readonly IBrugerRepo _brugerRepo;
        private readonly TokenProvider _tokenProvider;
        private readonly IBrugerService _brugerservice;

        public BrugerController(IBrugerRepo brugerRepo, TokenProvider tokenProvider,IBrugerService brugerservice)
        {
            _brugerRepo = brugerRepo;
            _tokenProvider = tokenProvider;
            _brugerservice = brugerservice;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Bruger>>> GetAll()
        {
            var bruger = await _brugerRepo.GetAllAsync();
            if(bruger==null)
            {
                return NoContent();
            }
            return Ok(bruger);
        }

        [HttpPost]
        public async Task<ActionResult<Bruger>> Add(BrugerCreateDTO brugerDto)
        {
            if(brugerDto==null)
            {
                return BadRequest("Bruger cannot be null");
            }
            var newBruger = new Bruger
            {
                Navn = brugerDto.Navn,
                Email = brugerDto.Email,
                Password = brugerDto.Password
            };
            await _brugerRepo.AddAsync(newBruger);
            await _brugerRepo.SaveChangesAsync();
            return Ok(newBruger);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody]LoginModelDTO loginModelDTO)
        {
            if (loginModelDTO == null)
            {
                return BadRequest("Login model cannot be null");
            }
            var bruger=await _brugerRepo.AuthenticateAsync(loginModelDTO.Email,loginModelDTO.Password);
            if(bruger==null)
            {
                return Unauthorized("Invalid email or password");
            }

            var token=_tokenProvider.Create(bruger);

            return Ok(token);
            
        }

        [HttpGet("name"),Authorize]
        public async Task<ActionResult<string>> GetUserName()
        {
            try
            {
                var brugerName=await _brugerservice.GetBrugerNavnAsync(User);
                return Ok(brugerName);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid user ID");
            }
        }

        [HttpGet("email"),Authorize]
        public async Task<ActionResult<string>> GetUserEmail()
        {
            try
            {
                var brugerEmail=await _brugerservice.GetBrugerEmailAsync(User);
                return Ok(brugerEmail);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid user ID");
            }
        }
    }
}