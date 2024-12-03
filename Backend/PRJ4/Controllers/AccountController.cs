using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging; // For logging
using Microsoft.EntityFrameworkCore; // For CookDTO
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using PRJ4.Data;
using PRJ4.Models;
using PRJ4.DTOs;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Repositories;
using PRJ4.Services; // Add this line for IRevocationService


namespace PRJ4.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AccountController> _logger; // For logging
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _config;
    private readonly SignInManager<ApiUser> _signInManager;
    private readonly IRevocationService _revocationService;

    public AccountController(ApplicationDbContext context, ILogger<AccountController> logger, 
            UserManager<ApiUser> userManager, IConfiguration config, SignInManager<ApiUser> signInManager, IRevocationService revocationService)
    {
        _context = context;
        _logger = logger; // For logging
        _userManager = userManager;
        _config = config;
        _signInManager = signInManager;
        _revocationService = revocationService;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register(RegisterDTO registerDTO)
    {
        Console.WriteLine("Registering user");
        Console.WriteLine("Model state is invalid");
        foreach (var entry in ModelState)
        {
            Console.WriteLine($"Key: {entry.Key}, Errors: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
        }

        Console.WriteLine($"Register DTO: {registerDTO}");

        try
        {
            Console.WriteLine("Model state is valid");
            var newUser=new ApiUser();
            newUser.UserName=registerDTO.Email;
            newUser.Email=registerDTO.Email;
            newUser.FullName=registerDTO.Fornavn+" "+registerDTO.Efternavn;

            var result=await _userManager.CreateAsync(newUser,registerDTO.Password);
            if(result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.",
                newUser.UserName,newUser.Email);
                return StatusCode(201, 
                new { message = $"User {newUser.UserName} created successfully" });

            }
            else
                throw new Exception(
                string.Format("Error: {0}", string.Join(" ",
                result.Errors.Select(e => e.Description))));       
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails();
            exceptionDetails.Detail = e.Message;
            exceptionDetails.Status =
            StatusCodes.Status500InternalServerError;
            exceptionDetails.Type =
            "https:/ /tools.ietf.org/html/rfc7231#section-6.6.1";
            return StatusCode(
            StatusCodes.Status500InternalServerError,
            exceptionDetails);
        }
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult> Login(LoginModelDTO loginDTO)
    {
        try
        {
            if(ModelState.IsValid)
            {
                var user=await _userManager.FindByNameAsync(loginDTO.UserName);
                if(user==null||!await _userManager.CheckPasswordAsync(user,loginDTO.Password))
                {
                    throw new Exception("Invalid login attempt.");
                }
                else
                {
                    var SigingCredentials=new SigningCredentials(new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(_config["JWT:SigningKey"])),SecurityAlgorithms.HmacSha256);
                    
                    var claims=new List<Claim>();
                    claims.Add(new Claim (JwtRegisteredClaimNames.Sub,user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name,user.UserName));
                    claims.Add(new Claim(ClaimTypes.Email,user.Email));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));

                    
                    var roleClaim=(await _userManager.GetClaimsAsync(user)).FirstOrDefault(c=>c.Type==ClaimTypes.Role);
                    if(roleClaim!=null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role,roleClaim.Value));
                    }

                    // Check if the user has the "IsAdmin" claim
                    // var isAdminClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "IsAdmin");
                    // if (isAdminClaim != null && isAdminClaim.Value == "true")
                    // {
                    //     claims.Add(new Claim("IsAdmin", "true"));
                    // }
                    // else
                    // {
                    //     claims.Add(new Claim("IsAdmin", "false"));
                    // }

                    var jwtObject=new JwtSecurityToken(
                        issuer:_config["JWT:Issuer"],
                        audience:_config["JWT:Audience"],
                        claims:claims,
                        expires:DateTime.Now.AddMinutes(300),
                        signingCredentials:SigingCredentials
                    );
                    var jwtString=new JwtSecurityTokenHandler()
                    .WriteToken(jwtObject);
                    return Ok(new { token = jwtString });
                }
            }
            else
            {
                var details = new ValidationProblemDetails(ModelState);
                details.Type =
                "https:/ /tools.ietf.org/html/rfc7231#section-6.5.1";
                details.Status = StatusCodes.Status400BadRequest;
                return new BadRequestObjectResult(details);
            }
            
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails();
            exceptionDetails.Detail = e.Message;
            exceptionDetails.Status =
            StatusCodes.Status401Unauthorized;
            exceptionDetails.Type =
            "https:/ /tools.ietf.org/html/rfc7231#section-6.6.1";
            return StatusCode(
                StatusCodes.Status401Unauthorized, exceptionDetails);
        }
    }

    [HttpPost]
    [Route("Logout")]
    [Authorize]
    public async Task<ActionResult>Logout()
    {

        var claims = User.Claims;
        var userIdClaim = claims.FirstOrDefault(c=>c.Type.Split('/').Last()=="nameidentifier");
        if(userIdClaim.Value!=null)
        {
            await _revocationService.RevokeRefreshTokenAsync(userIdClaim.Value);
        }
        
        await _signInManager.SignOutAsync();
        return StatusCode(StatusCodes.Status200OK,"User logged out");
    }

    [HttpGet]
    [Route("WhoAmI")]
    [Authorize]
    public async Task<ActionResult> WhoAmI()
    {
        var claims = User.Claims;

        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last()=="nameidentifier");

        if(userIdClaim.Value ==null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized,"User not found");
        }

        try
        {
            var user=await _userManager.FindByIdAsync(userIdClaim.Value);
            if(user==null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized,"User not found");
            }
            return StatusCode(StatusCodes.Status200OK,user);
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails();
            exceptionDetails.Detail = e.Message;
            exceptionDetails.Status =
            StatusCodes.Status500InternalServerError;
            exceptionDetails.Type =
            "https:/ /tools.ietf.org/html/rfc7231#section-6.6.1";
            return StatusCode(
                StatusCodes.Status500InternalServerError, exceptionDetails);
        }
    }

    // [HttpDelete]
    // [Route("Delete/{id}")]
    // //could be admin only
    // public async Task<IActionResult> Delete(string id)
    // {
    //     try
    //     {
    //         _logger.LogInformation("Deleting Bruger");
                
    //         _context.Remove(id);
    //         await _context.SaveChangesAsync();

    //         _logger.LogInformation("Successfully deleted");
    //         return NoContent();
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError("Error deleting ");
    //         return BadRequest(ex.Message);
    //     }
    // }
}
