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
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal; // For QueryHelpers


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
    private readonly EmailService _emailService;

    public AccountController(ApplicationDbContext context, ILogger<AccountController> logger, 
            UserManager<ApiUser> userManager, IConfiguration config,
            SignInManager<ApiUser> signInManager, IRevocationService revocationService,
            EmailService emailService)
    {
        _context = context;
        _logger = logger; // For logging
        _userManager = userManager;
        _config = config;
        _signInManager = signInManager;
        _revocationService = revocationService;
        _emailService = emailService;
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

    // [HttpPost]
    // [Route("SendEmail")]
    // [AllowAnonymous]

    // public ActionResult SendEmail(string to,string subject,string text)
    // {
    //     if (string.IsNullOrEmpty(to) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(text))
    //     {
    //         return StatusCode(StatusCodes.Status400BadRequest, "Missing parameters");
    //     }
        
    //     var response = _emailService.SendSimpleMessage(
    //         to, subject, text);
        
    //     if (response.IsSuccessful)
    //     {
    //         return StatusCode(StatusCodes.Status200OK, "Email sent successfully");
    //     }
    //     else
    //     {
    //         return StatusCode(StatusCodes.Status500InternalServerError, "Email could not be sent");
    //     }
    // }

    [HttpPost]
    [Route("forgotpassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPassword)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        
        var user = await _userManager.FindByEmailAsync(forgotPassword.Email);

        if(user==null)
            return BadRequest("User not found");
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var param = new Dictionary<string, string?>
        {
            {"token", token},
            {"email", forgotPassword.Email}
        };

        var callback = QueryHelpers.AddQueryString(forgotPassword.ClientURI!, param);
        
        var response = _emailService.SendSimpleMessage(
           user.Email, "Reset Password", callback);

        if (response.IsSuccessful)
        {
            return StatusCode(StatusCodes.Status200OK, "Email sent successfully" + token);
            Console.WriteLine(token);
        }
        else
            return StatusCode(StatusCodes.Status500InternalServerError, "Email could not be sent");


    }

    [HttpPost]
    [Route("resetpassword")]
    [AllowAnonymous]

    public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDTO resetPassword)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        
        var user = await _userManager.FindByEmailAsync(resetPassword.Email);

        if(user==null)
            return BadRequest("User not found");

        var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);

        if(result.Succeeded)
            return StatusCode(StatusCodes.Status200OK, "Password reset successfully");
        else
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }
    }

    [HttpPost]
    [Route("changepassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDTO changePasswordDTO)
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

            
            if(!ModelState.IsValid)
                return BadRequest();
            
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);

            if(result.Succeeded)
                return StatusCode(StatusCodes.Status200OK, "Password changed successfully");
            else
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

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
}
