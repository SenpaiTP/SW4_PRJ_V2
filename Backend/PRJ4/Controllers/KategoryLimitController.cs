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
    public KategoryLimitController(IKategoryLimitService kategoryLimitService)
    {
        _kategoryLimitService = kategoryLimitService;
    }

    // GET: api/KategoryLimit/{Id}
    [HttpGet()]
    public async Task<ActionResult<List<KategoryLimitResponseDTO>>> GetAll()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }
        var kategoryLimit = await _kategoryLimitService.GetAllKategoryLimits(userId); 
        return kategoryLimit; 
    }  
  

    // GET: api/KategoryLimit/{Id}
    [HttpGet("{id}")]
    public async Task<ActionResult<KategoryLimitResponseDTO>> GetById(int id)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }
        var kategoryLimit = await _kategoryLimitService.GetByIdKategoryLimits(id, userId); 
        return kategoryLimit; 
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
    
        var kategoryLimit = await _kategoryLimitService.AddKategoryLimitAsync(limitDTO, userId);
        
        return kategoryLimit;
    }

}