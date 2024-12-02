using PRJ4.Models;  
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;
using PRJ4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


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
        if(userIdClaim.Value != null)
        {
            return userIdClaim.Value;
        }
        return null;
    }
    private readonly IKategoryLimitService _kategoryLimitService;
    public KategoryLimitController(IKategoryLimitService kategoryLimitService)
    {
        _kategoryLimitService = kategoryLimitService;
    }
  

    // GET: api/KategoryLimit/{Id}
    [HttpGet("{id}")]
    public async Task<KategoryLimitGetDTO> GetKategorybyIdLimit(int id)
    {
        var budget = await _kategoryLimitService.GetByIdKategoryLimitAsync(id); 
        return budget; 
    }

    // POST
    [HttpPost] 
    public async Task<KategoryLimitGetDTO> AddKategoryLimit(KategoryLimitReturnDTO limitDTO) 
    {
        var user = GetUserId();
    
        var kategoryLimit = await _kategoryLimitService.AddKategoryLimitAsync(user, limitDTO);
        
        return kategoryLimit;
    }

}