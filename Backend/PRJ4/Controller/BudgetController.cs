using PRJ4.Models;  
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;
using PRJ4.Services;


namespace PRJ4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class budgetController : ControllerBase
{
    private readonly IBudgetGoalService _budgetGoalService;
    public budgetController(IBudgetGoalService budgetService)
    {
        _budgetGoalService = budgetService;
    }

    // GET
    [HttpGet]
    public async Task<List<BudgetResponseDTO>> GetAllBudgets() 
    {
        var budget = await _budgetGoalService.GetAllBudgetGoalsAsync();
        return budget;
    }   

    // GET: api/budget/{Id}
    [HttpGet("{id}")]
    public async Task<BudgetResponseDTO> GetBudget(int id)
    {
        var budget = await _budgetGoalService.GetByIdBudgetGoalAsync(id); 
        return budget; 
    }

    // GET: api/budget/{userId}
    [HttpGet("user/{userId}")]
    public async Task<List<BudgetResponseDTO>> GetBudgetsByUserId(int userId)
    {
        var budget = await _budgetGoalService.GetByUserIdBudgetGoalAsync(userId); 
        return budget; 
    }

    // POST
    [HttpPost] 
    public async Task<BudgetCreateDTO> AddBudget(int brugerId, BudgetCreateDTO budgetDTO) 
    {
        var budget = await _budgetGoalService.AddBudgetGoalAsync(brugerId, budgetDTO); 
        
        return budget;
    }

    // UPDATE: api/budget/{id}
    [HttpPut("{id}/update")]
    public async Task<BudgetCreateDTO> UpdateBudget(int id, BudgetCreateDTO budgetDTO) 
    {
        var budget = await _budgetGoalService.UpdateBudgetGoalAsync(id, budgetDTO);
        return budget; 
    }

    // Delete: api/budget/{id}
    [HttpDelete("{id}/delete")]
    public async Task<BudgetCreateDTO> Delete(int id)
    {
        var budget = await _budgetGoalService.DeleteBudgetAsync(id); 
        return budget;
    }
}