using PRJ4.Models;  
using Microsoft.AspNetCore.Mvc;
using PRJ4.Services;
using Microsoft.AspNetCore.Authorization;

namespace PRJ4.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class KategoryLimitController : ControllerBase
{
    
    private string GetUserId() 
    {
        var claims = Request.HttpContext.User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last() == "nameidentifier");
        if (userIdClaim == null)
        {
            return null;
        }
        return userIdClaim.Value;
    }  
    private readonly IKategoryLimitService _kategoryLimitService;
    private readonly ILogger<KategoryLimitController> _logger;
    public KategoryLimitController(IKategoryLimitService kategoryLimitService, ILogger<KategoryLimitController> logger)
    {
        _kategoryLimitService = kategoryLimitService;
        _logger = logger; 
    }

    // GET
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to get all category limits
        try
        {
            var kategoryLimit = await _kategoryLimitService.GetAllKategoryLimits(userId);
            return Ok(kategoryLimit); // Returns statuskode a list of all category limits
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while getting category limit by user with id: {userId}.", userId);
            return StatusCode(500, new { Message = "An error occurred while getting category limits"});
        }
    }  
  

    // GET: api/KategoryLimit/{Id}
    [HttpGet("{id}")]
    public async Task<ActionResult<KategoryLimitResponseDTO>> GetById(int id)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID is null.");
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to get category limit by id
        try
        {
            var kategoryLimit = await _kategoryLimitService.GetByIdKategoryLimits(id, userId); 
            return Ok(kategoryLimit); // Returns statuskode a list of all category limits
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while getting category limit with id {id} by user with id: {userId}.", id , userId);
            return StatusCode(500, new { Message = "An error occurred while getting category limit."});
        }
    }  

    // POST
    [HttpPost]  
    public async Task<ActionResult<KategoryLimitResponseDTO>> AddKategoryLimit(KategoryLimitCreateDTO limitDTO) 
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        try
        {
            var kategoryLimit = await _kategoryLimitService.AddKategoryLimitAsync(limitDTO, userId); 
            return Ok(kategoryLimit); // Returns statuskode a list of all category limits
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while creating category limit by user with id: {userId}.", userId);
            return StatusCode(500, new { Message = "An error occurred while creating category limit."});
        }
    }

}