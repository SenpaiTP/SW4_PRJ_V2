 
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;
using PRJ4.Services;
using Microsoft.AspNetCore.Authorization;


namespace PRJ4.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BudgetController : ControllerBase  
{

    private readonly IBudgetService _BudgetService;
    private readonly ILogger<BudgetController> _logger;
    public BudgetController(IBudgetService BudgetService,ILogger<BudgetController> logger)
    {
        _BudgetService = BudgetService;
        _logger = logger;  

    }

     // Method to extract user ID from claims
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

    // GET: api/Budget/{budgetId} - Get users budget by id
    [HttpGet("{budgetId}")]
    public async Task<IActionResult> GetById(int budgetId)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID is missing.");
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to get budget with budgetId
        try
        {
            _logger.LogInformation($"Fetching all budgets with ID: {budgetId} for user with ID: {userId}");
            var budget = await _BudgetService.GetBudgetByIdAsync( budgetId,  userId);
            return Ok(budget); // Returns statuskode with a specifik budget
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Budget not found for user with ID {userId}: {ex.Message}");
            return NotFound(new { Message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid data while getting budget for user with id {userId}:  {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"No acess for user with ID {userId}: {ex.Message}");
            return Forbid();
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError($"Error getting budget for user with id {userId}: {ex.Message}");
            return StatusCode(500, new { Message = "An error occurred while getting budget"});
        }

        
    }

    // GET: api/Budget/AllBudgets - Get all budgets for a user
    [HttpGet("AllBudgets")]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID is missing.");
            return Unauthorized(new { Message = "User not authenticated" });
        }
        //Try to get all budgets
        try
        {
            _logger.LogInformation($"Fetching all budgets for user with ID: {userId}");
            var budget = await _BudgetService.GetAllBudgetsAsync(userId);
            return Ok(budget); 
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Budgets not found for user with ID {userId}");
            return NotFound(new { Message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid data while getting budgets for user with ID {userId}: {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"No access for user with ID {userId}: {ex.Message}");
            return Forbid();
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while getting all budgets by user {userId}.", userId);
            return StatusCode(500, new { Message = "An error occurred while getting all budgets"});
        }
    }

    // POST: api/Budget/NewBudget - Create a new budget
    [HttpPost("NewBudget")] 
    public async Task<ActionResult<BudgetCreateDTO>> Add(BudgetCreateDTO budgetDTO) 
    {  
        //Get user
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID is missing.");
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to add new budget goal
        try
        {
            var budget = await _BudgetService.AddBudgetAsync(userId,budgetDTO);
            return Ok(budget); // Returns statuskode with created budget
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid data while adding budgets for user with ID {userId}: {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while adding a budget by user {userId}.", userId);
            return StatusCode(500, new { Message = "An error occurred while creating the budget"});
        }
    }


    // UPDATE: api/Budget/Budget - Update the values in an existing budget
    [HttpPut("Budget")]
    public async Task<ActionResult<BudgetUpdateDTO>> UpdateBudget(int budgetId, BudgetUpdateDTO budgetDTO) 
    {
        // get and check for user.
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID is missing.");
            return Unauthorized(new { Message = "User not authenticated" });
        } 

        //Try to update budget goal
        try
        {
            var budget = await _BudgetService.UpdateBudgetAsync( budgetId,  userId,  budgetDTO);
            return Ok(budget); // Returns statuskode with updated budget
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid data while updating budget for user with ID {userId}: {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Budgets not found for user with ID {userId}: {ex.Message}");
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while updating a budget by user {userId}.", userId);
            return StatusCode(500, new { Message = "An error occurred while updating the budget"});
        }
    }

    // Delete: api/Budget/Budget - delete an existing budget
    [HttpDelete("Budget")]
    public async Task<IActionResult> DeleteBudget(int budgetId)
    {
        // get and check for user.
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID is missing.");
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to delete budget 
        try
        {
            await _BudgetService.DeleteBudgetAsync( budgetId,  userId);
            return Ok(new { Message = $"Budget with ID {budgetId} has been successfully deleted." });

        }
         catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid data while deleting budget for user with ID {userId}: {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Budgets not found for user with ID {userId}: {ex.Message}");
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            // Returns error message.
            _logger.LogError(ex, $"An error occurred while deleting budget by user {userId}.", userId);
            return StatusCode(500, new { Message = "An error occurred while deleting the budget"});
        }
    }
}