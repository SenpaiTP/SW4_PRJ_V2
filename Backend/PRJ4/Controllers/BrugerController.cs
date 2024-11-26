// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using PRJ4.Infrastructure;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Authorization;
// using PRJ4.Data;
// using PRJ4.Models;  
// using PRJ4.Repositories;
// using PRJ4.DTOs;
// using PRJ4.Infrastructure;
// using PRJ4.Services;


// namespace PRJ4.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]

//     public class BrugerController:ControllerBase
//     {
//         private readonly IBrugerRepo _brugerRepo;
//         private readonly TokenProvider _tokenProvider;
//         private readonly IBrugerService _brugerservice;

//         public BrugerController(IBrugerRepo brugerRepo, TokenProvider tokenProvider,IBrugerService brugerservice)
//         {
//             _brugerRepo = brugerRepo;
//             _tokenProvider = tokenProvider;
//             _brugerservice = brugerservice;
//         }
        
//         [HttpGet]
//         [Authorize]
//         public async Task<ActionResult<IEnumerable<Bruger>>> GetAll()
//         {
//             var bruger = await _brugerRepo.GetAllAsync();
//             if(bruger==null)
//             {
//                 return NoContent();
//             }
//             return Ok(bruger);
//         }

//         [HttpPost]
//         public async Task<ActionResult<Bruger>> Add(BrugerCreateDTO brugerDto)
//         {
//             if(brugerDto==null)
//             {
//                 return BadRequest("Bruger cannot be null");
//             }
//             var newBruger = new Bruger
//             {
//                 Fornavn = brugerDto.Fornavn,
//                 Efternavn = brugerDto.Efternavn,
//                 Email = brugerDto.Email,
//                 Password = brugerDto.Password
//             };
//             await _brugerRepo.AddAsync(newBruger);
//             await _brugerRepo.SaveChangesAsync();
//             return Ok(newBruger);
//         }

//         [HttpPost("login")]
//         public async Task<ActionResult<string>> Login([FromBody]LoginModelDTO loginModelDTO)
//         {
//             if (loginModelDTO == null)
//             {
//                 return BadRequest("Login model cannot be null");
//             }
//             var bruger=await _brugerRepo.AuthenticateAsync(loginModelDTO.Email,loginModelDTO.Password);
//             if(bruger==null)
//             {
//                 return Unauthorized("Invalid email or password");
//             }

//             var token=_tokenProvider.Create(bruger);

//             return Ok(token);
            
//         }

//         // [HttpGet("whoami"),Authorize]
//         // public async Task<ActionResult<string>> GetUserInfo()
//         // {
//         //     try
//         //     {
//         //         var bruger=await _brugerservice.GetBrugerAsync(User);
//         //         return Ok(bruger);
//         //     }
//         //     catch (KeyNotFoundException)
//         //     {
//         //         return NotFound("User not found");
//         //     }
//         //     catch (ArgumentException)
//         //     {
//         //         return BadRequest("Invalid user ID");
//         //     }
//         // }

//         // [HttpPut("forgotPassword"),Authorize]
//         // public async Task<ActionResult<string>> ForgotPassword([FromBody]ForgotPasswordDTO forgotPasswordDTO)
//         // {
//         //     if (forgotPasswordDTO == null)
//         //     {
//         //         return BadRequest("Forgot password model cannot be null");
//         //     }
//         //     var bruger=await _brugerRepo.GetByEmailAsync(forgotPasswordDTO.Email);
//         //     if(bruger==null)
//         //     {
//         //         return NotFound("User not found");
//         //     }
//         //     var newPassword=forgotPasswordDTO.NewPassword;
//         //     bruger.Password=newPassword;
//         //     await _brugerRepo.SaveChangesAsync();
//         //     return Ok("Password changed successfully");
//         // }
//     }
// }