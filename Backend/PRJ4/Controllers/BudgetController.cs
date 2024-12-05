  
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;
using PRJ4.Services;
using PRJ4.Repositories;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Models;



namespace PRJ4.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class budgetController : ControllerBase
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
    private readonly IBudgetGoalService _budgetGoalService;
    private readonly IVudgifterService _vudgifterService;
    private readonly IVudgifter _vudgifterRepo;
    private readonly IKategori _kategoryRepo;
    
    public budgetController(IBudgetGoalService budgetService, IVudgifterService vudgifterService, IVudgifter vudgifterRepo, IKategori kategoryRepo)
    {
        _budgetGoalService = budgetService;
        _vudgifterService = vudgifterService;
        _vudgifterRepo = vudgifterRepo;
        _kategoryRepo = kategoryRepo;
    }

    // GET - denne funktion skal ikke bruges!!!
    [HttpGet]
    public async Task<IActionResult> GetAllBudgets() 
    {
        var budget = await _budgetGoalService.GetAllBudgetGoalsAsync();
        return Ok(budget);
    }   

    // GET: api/budget/{Id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdBudget(int id)
    {
        var user = GetUserId();
        if (user == null)
        {
            return Unauthorized(new { message = "User not authenticated."});
        }
        var budget = await _budgetGoalService.GetByIdBudgetGoalAsync(id, user); 
        return Ok(budget); 
    }

    // GET: api/budget/{userId}
    [HttpGet("user")]
    public async Task<IActionResult> GetBudgetsByUserId()
    {
        var user = GetUserId();
        if (user == null)
        {
            return BadRequest(new { message = "User identifier is missing."});
        }
        var budget = await _budgetGoalService.GetAllByUserIdBudgetGoalAsync(user); 
        return Ok(budget); 
    }

    // GET 
    [HttpGet("getSavings")] 
    public async Task<List<SavingDTO>> GetSaving(int budgetId) 
    {
        var user = GetUserId();
        if (user == null)
        {
            throw new Exception($"User not authenticated");
        }
        var budget = await _budgetGoalService.GetAllSavingsAsync( budgetId,  user);

        return budget;
    }

    // POST
    [HttpPost] 
    public async Task<ActionResult<BudgetCreateUpdateDTO>> AddBudget(BudgetCreateUpdateDTO budgetDTO) 
    {  
        //Get user
        var user = GetUserId();
        if (user == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to add new budget goal
        try
        {
            var budget = await _budgetGoalService.AddBudgetGoalAsync(user, budgetDTO);
            return Ok(budget); // Returns statuskode with created budget
        }
        catch (Exception ex)
        {
            // Returns error message.
            return StatusCode(500, new { Message = "An error occurred while creating the budget", Details = ex.Message });
        }
    }

    // POST 
    [HttpPost("addSaving")] 
    public async Task<VudgifterResponseDTO> AddSaving(SavingDTO savingDTO, int budgetId) 
    {
        var user = GetUserId();
        if (user == null)
        {
            throw new Exception($"User not authenticated");
        }
        
        var saving = await _budgetGoalService.AddSavingAsync(user, savingDTO, budgetId);
        
        return saving;
    }


    // UPDATE: api/budget/{id}
    [HttpPut("{id}/update")]
    public async Task<BudgetCreateUpdateDTO> UpdateBudget(int id, BudgetCreateUpdateDTO budgetDTO) 
    {
        var budget = await _budgetGoalService.UpdateBudgetGoalAsync(id, budgetDTO);
        return budget; 
    }

    // Delete: api/budget/{id}
    [HttpDelete("{id}/delete")]
    public async Task<BudgetCreateUpdateDTO> Delete(int id)
    {
        var budget = await _budgetGoalService.DeleteBudgetAsync(id); 
        return budget;
    }
}