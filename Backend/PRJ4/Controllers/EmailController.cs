// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.Extensions.Logging; // For logging
// using Microsoft.EntityFrameworkCore; // For CookDTO
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using Microsoft.IdentityModel.Tokens;
// using PRJ4.Data;
// using PRJ4.Models;
// using PRJ4.DTOs;
// using Microsoft.AspNetCore.Authorization;
// using PRJ4.Repositories;
// using SendingEmails;
// public class EmailController : ControllerBase
// {
//     private readonly IEmailSender _emailSender;

//     // Constructor injection of IEmailSender
//     public EmailController(IEmailSender emailSender)
//     {
//         _emailSender = emailSender;
//     }

//     [HttpGet]
//     [Route("SendEmail")]
//     [AllowAnonymous]
//     public async Task<IActionResult> SendEmail(string email, string subject, string content)
//     {
//         try
//         {
//             await _emailSender.SendEmail(email, subject, content);
//             return Ok("Email sent successfully");
//         }
//         catch (Exception e)
//         {
//             return StatusCode(500, $"Internal server error: {e.Message}");
//         }
//     }
// }
