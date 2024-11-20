using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using PRJ4.Services;
using PRJ4.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Infrastructure;
using Microsoft.VisualBasic;
using PRJ4.Services.BudgetService;

namespace PRJ4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class budgetController : ControllerBase
{
    private readonly IBudgetRepo _budgetRepository;
    private readonly IBrugerRepo _brugerRepository;

    private readonly IBudgetAmountService _budgetAmountService;

    public budgetController(IBudgetRepo budgetRepository, IBrugerRepo brugerRepository, IBudgetAmountService budgetService)
    {
        _budgetRepository = budgetRepository;
        _brugerRepository = brugerRepository;
        _budgetAmountService = budgetService;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetAllBudgets() 
    {
        //var budget = await _budgetRepository.GetAllAsync();
        var budget = await _budgetAmountService.GetAllBudgetsWithMonthlySavingsAsync();
        if (budget == null)
        {
            return NotFound($"No savings made");
        }

        return Ok(budget);
    }   

    // GET: api/budget/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBudget(int id)
    {
        var budget = await _budgetAmountService.GetByIdBudgetWithMonthlySavingsAsync(id);
        // var budget = await _budgetRepository.GetByIdAsync(id); 
        // if (budget == null)
        // {
        //     return BadRequest($"Savings with id {id} not found."); 
        // }

        // int monthlysaving = _budgetService.CalculateMonthlySavings(budget.SavingsGoal,budget.BudgetStart,budget.BudgetSlut);

        // var budgetReturn = new BudgetReturnDTO
        // {
        //     SavingsGoal = budget.SavingsGoal,
        //     BugdetStart = budget.BudgetStart,
        //     BugdetSlut = budget.BudgetSlut,
        //     MonthlySavingsAmount = monthlysaving
        // };

        return Ok(budget); 
    }

    

    // POST
    [HttpPost] 
    public async Task<IActionResult> AddBudget(int brugerId, BudgetDTO budgetDTO) 
    {
        //Tjek om bruger eksisterer
        Bruger bruger = await _brugerRepository.GetByIdAsync(brugerId);
        if (bruger == null)
        {
            throw new Exception("Bruger findes ikke.");
        }

        if (budgetDTO == null) 
        {
            return BadRequest("Savings values is not valid");
        }
            
        var budget = new Budget
        {
            BrugerId = brugerId,
            SavingsGoal = budgetDTO.SavingsGoal,
            BudgetStart = budgetDTO.BudgetStart,
            BudgetSlut = budgetDTO.BudgetSlut
        };
           
        var createdBudget =  await _budgetRepository.AddAsync(budget);
        await _budgetRepository.SaveChangesAsync(); 

        return CreatedAtAction(nameof(GetBudget), new { id = createdBudget.BudgetId }, budget);

    }

    // UPDATE: api/budget/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget(int id, BudgetDTO budgetDTO) 
    {
        //Check if DTO object contain values 
        if (budgetDTO == null)
        {
            return BadRequest("Invalid budget data.");
        }

           //Retrieve existing budget
        var existingBudget = await _budgetRepository.GetByIdAsync(id); 
        if (existingBudget == null)
        {
            return NotFound($"Budget with id {id} not found.");
        }
 
        existingBudget.SavingsGoal = budgetDTO.SavingsGoal;
        existingBudget.BudgetStart = budgetDTO.BudgetStart;
        existingBudget.BudgetSlut = budgetDTO.BudgetSlut;
            
        //Try updating the old budget
        try
        {
            await _budgetRepository.Update(existingBudget); 
        }
        catch(Exception ex)
        {
            Console.WriteLine($"HEYYY: Error updating budget: {ex.Message}");
            return StatusCode(500, "An error occurred while updating the budget.");
        }
        await _budgetRepository.SaveChangesAsync();    

        return Ok(existingBudget); 
    }

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var budget = await _budgetRepository.GetByIdAsync(id); 
        if (budget == null)
        {
            return NotFound($"Budget with id {id} not found.");
        }
        
        try
        {
            await _budgetRepository.Delete(id);
            await _budgetRepository.SaveChangesAsync(); 
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HEYYY: Error deleting budget: {ex.Message}");
            return StatusCode(500, "An error occurred while deleting the budget.");
        }
        
    }



}