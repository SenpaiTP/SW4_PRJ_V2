  
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
public class BudgetController : ControllerBase
{

    private readonly IBudgetGoalService _budgetGoalService;
    private readonly ILogger<BudgetController> _logger;
    public BudgetController(IBudgetGoalService budgetGoalService,ILogger<BudgetController> logger)
    {
        _budgetGoalService = budgetGoalService;
        _logger = logger;

    }
    // Retrieves the userId from the claims in the http-request. If no userId is found it returns null.
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

    // GET: api/Budget - Get all budgets for all users. Should not be used to check budget - no useridentification
    [HttpGet]
    public async Task<IActionResult> GetAllBudgets() 
    {
        var budget = await _budgetGoalService.GetAllBudgetGoalsAsync();
        return Ok(budget);
    }   

    // GET: api/Budget/{budgetId} - Get users budget by id
    [HttpGet("{budgetId}")]
    public async Task<IActionResult> GetByIdBudget(int budgetId)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to get budget with budgetId
        try
        {
            var budget = await _budgetGoalService.GetByIdBudgetGoalAsync(budgetId, userId);
            return Ok(budget); // Returns statuskode with a specifik budget
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while getting budget with id {budgetId} by user {userId}.", budgetId, userId);
            return StatusCode(500, new { Message = "An error occurred while getting budget"});
        }

        
    }

    // GET: api/Budget/AllBudgets - Get all budgets for a user
    [HttpGet("AllBudgets")]
    public async Task<IActionResult> GetBudgetsByUserId()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

     
         //Try to get all budgets
        try
        {
            var budget = await _budgetGoalService.GetAllByUserIdBudgetGoalAsync(userId);
            return Ok(budget); // Returns statuskode with all savings
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while getting all budgets by user {userId}.", userId);
            return StatusCode(500, new { Message = "An error occurred while getting all budgets"});
        }
    }

    // GET: api/Budget/AllSavings - Get all savings for a user
    [HttpGet("AllSavings")] 
    public async Task<ActionResult<List<SavingDTO>>> GetSaving(int budgetId) 
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to get all savings
        try
        {
            var savings = await _budgetGoalService.GetAllSavingsAsync( budgetId,  userId);
            return Ok(savings); // Returns statuskode with all savings
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while getting all savings for budget with budgetId {budgetId} by user {userId}.", budgetId, userId);
            return StatusCode(500, new { Message = "An error occurred while getting all savings" });
        }
    }

    // POST: api/Budget/NewBudget - Create a new budget
    [HttpPost("NewBudget")] 
    public async Task<ActionResult<BudgetCreateUpdateDTO>> AddBudget(BudgetCreateUpdateDTO budgetDTO) 
    {  
        //Get user
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to add new budget goal
        try
        {
            var budget = await _budgetGoalService.AddBudgetGoalAsync(userId, budgetDTO);
            return Ok(budget); // Returns statuskode with created budget
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while creating a budget by user {userId}.", userId);
            return StatusCode(500, new { Message = "An error occurred while creating the budget"});
        }
    }

    // POST api/Budget/NewSaving/{BudgetId} - Add money to a saving budget.
    [HttpPost("NewSaving/{budgetId}")] 
    public async Task<ActionResult<VudgifterResponseDTO>> AddSaving( int budgetId, SavingDTO savingDTO) 
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        
        try //Try to add money to saving
        {
            var saving = await _budgetGoalService.AddSavingAsync( budgetId, userId, savingDTO);
            return Ok(saving); // Returns statuskode with the saving
        }
        catch (Exception ex) // Returns error message if not possible
        {
            _logger.LogError(ex, $"An error occurred while adding a saving for budgetId {budgetId} by user {userId}. SavingDTO: {@savingDTO}", budgetId, userId);
            return StatusCode(500, new { Message = "An error occurred while adding a saving"});
        }
    }


    // UPDATE: api/Budget/UpdateBudget - Update the values in an existing budget
    [HttpPut("UpdateBudget")]
    public async Task<ActionResult<BudgetCreateUpdateDTO>> UpdateBudget(int budgetId, BudgetCreateUpdateDTO budgetDTO) 
    {
        // get and check for user.
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        } 

        //Try to update budget goal
        try
        {
            var budget = await _budgetGoalService.UpdateBudgetGoalAsync(budgetId, budgetDTO);
            return Ok(budget); // Returns statuskode with updated budget
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, $"An error occurred while updating a budget for budgetId {budgetId} by user {userId}.", budgetId, userId);
            // Returns error message.
            return StatusCode(500, new { Message = "An error occurred while updating the budget." });
        }
    }

    // Delete: api/Budget/DeleteBudget - delete an existing budget
    [HttpDelete("DeleteBudget")]
    public async Task<IActionResult> Delete(int budgetId)
    {
        // get and check for user.
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to delete budget 
        try
        {
            await _budgetGoalService.DeleteBudgetAsync(budgetId);
            return Ok(new { Message = $"Budget with ID {budgetId} has been successfully deleted." });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting a budget for budgetId {budgetId} by user {userId}.", budgetId, userId);
            // Returns error message.
            return StatusCode(500, new { Message = "An error occurred while deleting the budget." });
        }
    }
}