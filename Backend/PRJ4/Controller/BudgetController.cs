using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Infrastructure;
namespace PRJ4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly BudgetRepo _budgetRepository;

    public BudgetController(BudgetRepo budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetAllBudgets() //Get all budgets
    {
        var budget = await _budgetRepository.GetAllAsync();

        if (budget == null)
        {
            return NotFound($"No budget made");
        }
        return Ok(budget);
    }

    // GET: api/budget/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBudget(int id)
    {
        var budget = await _budgetRepository.GetByIdAsync(id); 
        if (budget == null)
        {
            return BadRequest($"Budget with id {id} not found."); 
        }

        return Ok(budget); 
    }

    // POST
        [HttpPost] 
        public IActionResult AddBudget(BudgetDTO budgetDTO) //Add a budget
        {
            if (budgetDTO == null) 
            {
                return BadRequest("Budget values is not valid");
            }
            
            var budget = new Budget
            {
                BrugerId = budgetDTO.BrugerId,
                SavingsGoal = budgetDTO.SavingsGoal,
                MonthlySavingsAmount = budgetDTO.MonthlySavingsAmount,
                SavingsPeriodInMonths = budgetDTO.SavingsPeriodInMonths
            };

            var createdBudget = _budgetRepository.AddAsync(budget);

            return CreatedAtAction(nameof(GetBudget), new { id = createdBudget.Id }, createdBudget);

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

           //Opdate only relevant values
            existingBudget.BrugerId = budgetDTO.BrugerId ?? existingBudget.BrugerId; 
            existingBudget.SavingsGoal = budgetDTO.SavingsGoal;
            existingBudget.MonthlySavingsAmount = budgetDTO.MonthlySavingsAmount;
            existingBudget.SavingsPeriodInMonths = budgetDTO.SavingsPeriodInMonths;
            
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

            return Ok(existingBudget); 
        }



}