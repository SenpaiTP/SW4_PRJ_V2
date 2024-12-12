using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace PRJ4.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepo _budgetRepository;
        private readonly ISavingRepo _savingRepository;
        private readonly ILogger<BudgetService> _logger;

        public BudgetService(IBudgetRepo budgetRepository, ISavingRepo savingRepository, ILogger<BudgetService> logger)
        {
            _budgetRepository = budgetRepository;
            _savingRepository = savingRepository;
            _logger = logger;
        }

        // Add a new budget
        public async Task<BudgetAllResponseDTO> AddBudgetAsync(string userId, BudgetCreateDTO budgetDTO)
        {
            if (budgetDTO == null)
                throw new ArgumentException(nameof(budgetDTO), "Budget data cannot be null.");

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.");
            }
            if (string.IsNullOrEmpty(budgetDTO.BudgetName))
                throw new ArgumentException("Budget name cannot be null or empty.", nameof(budgetDTO.BudgetName));
            
            if (budgetDTO.SavingsGoal < 0)
                throw new ArgumentOutOfRangeException(nameof(budgetDTO.SavingsGoal), "Savings goal cannot be negative.");

            if (budgetDTO.BudgetSlut < DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentOutOfRangeException(nameof(budgetDTO.BudgetSlut), "Budget end date cannot be in the past.");

            var budget = new Budget
            {
                BudgetName = budgetDTO.BudgetName,
                BrugerId = userId,
                SavingsGoal = budgetDTO.SavingsGoal,
                BudgetStart = DateOnly.FromDateTime(DateTime.Now),
                BudgetSlut = budgetDTO.BudgetSlut,
                Savings = new List<Saving>() 
            };

            try
            {
                await _budgetRepository.AddAsync(budget); 
                await _budgetRepository.SaveChangesAsync();

                _logger.LogInformation($"Successfully added budget {budget.BudgetName} for user {userId}.");
                return new BudgetAllResponseDTO
                {
                    BudgetId = budget.BudgetId,
                    BudgetName = budget.BudgetName,
                    SavingsGoal = budget.SavingsGoal,
                    BudgetSlut = budget.BudgetSlut
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding budget: {ex.Message}");
                throw new InvalidOperationException("Could not save the new budget.");
            }
        }

        // Get a specific budget by ID
        public async Task<BudgetResponseDTO> GetBudgetByIdAsync(int budgetId, string userId)
        {
            if (budgetId <= 0)
                throw new ArgumentException(nameof(budgetId), "Invalid budget ID.");

            var budget = await _budgetRepository.GetByIdAsync(budgetId);
            if (budget == null) 
                throw new KeyNotFoundException($"Budget with ID {budgetId} not found.");
            if (budget.BrugerId != userId) 
                throw new InvalidOperationException($"Budget with ID {budgetId} does not belong to the user.");
            try
            {
                
                decimal moneySaved = await CalculateMoneySaved(budget.BudgetId);
                var monthlySavings = CalculateMonthlySavings(budget.SavingsGoal, budget.BudgetStart, budget.BudgetSlut, moneySaved);

                return new BudgetResponseDTO
                {
                    BudgetId = budget.BudgetId,
                    BudgetName = budget.BudgetName,
                    SavingsGoal = budget.SavingsGoal,
                    BudgetSlut = budget.BudgetSlut,
                    MonthlySavingsAmount = monthlySavings,
                    MoneySaved = moneySaved
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while retrieving budget with ID {budgetId}: {ex.Message}");
                throw new InvalidOperationException("Could not retrieve the budget.");
            }
        }

        // Get all budgets for a user
        public async Task<List<BudgetAllResponseDTO>> GetAllBudgetsAsync(string userId)
        {
            
                var budgets = await _budgetRepository.GetBudgetsForUserAsync(userId);
                if (budgets == null || !budgets.Any())
                    throw new KeyNotFoundException($"No budgets found for user");
                if (budgets.Any(b => b.BrugerId != userId))
                    throw new UnauthorizedAccessException($"One or more budgets do not belong to user");


            try
            {
                var budgetResponses = new List<BudgetAllResponseDTO>();
                foreach (var budget in budgets)
                {
                    
                    var budgetResponse = new BudgetAllResponseDTO
                    {
                        BudgetId = budget.BudgetId,
                        BudgetName = budget.BudgetName,
                        SavingsGoal = budget.SavingsGoal,
                        BudgetSlut = budget.BudgetSlut,
                    };

                    budgetResponses.Add(budgetResponse);
                }
                            return budgetResponses.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while retrieving all budgets for user: {ex.Message}");
                throw new InvalidOperationException("Could not retrieve the budgets.");
            }
        }

        // Update a budget's details
        public async Task<BudgetAllResponseDTO> UpdateBudgetAsync(int budgetId, string userId, BudgetUpdateDTO budgetUpdateDTO)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.");

            if (budgetUpdateDTO == null)
                throw new ArgumentException(nameof(budgetUpdateDTO), "Budget data cannot be null.");
            
            var budget = await _budgetRepository.GetByIdAsync(budgetId);
            if (budget == null)
                throw new KeyNotFoundException($"Budget with ID {budgetId} not found.");

            if (budget.BrugerId != userId)
                throw new UnauthorizedAccessException($"Budget with ID {budgetId} does not belong to user");

            if (string.IsNullOrEmpty(budgetUpdateDTO.BudgetName))
                throw new ArgumentException("Budget name cannot be null or empty.", nameof(budgetUpdateDTO.BudgetName));

            if (budgetUpdateDTO.SavingsGoal < 0)
                throw new ArgumentException(nameof(budgetUpdateDTO.SavingsGoal), "Savings goal cannot be negative.");

            if (budgetUpdateDTO.BudgetSlut < DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException(nameof(budgetUpdateDTO.BudgetSlut), "Budget end date must be in the future.");

            try
            {
                if (budgetUpdateDTO.BudgetName != null)
                    budget.BudgetName = budgetUpdateDTO.BudgetName;

                if (budgetUpdateDTO.SavingsGoal != 0)
                    budget.SavingsGoal = budgetUpdateDTO.SavingsGoal;

                if (budgetUpdateDTO.BudgetSlut != DateOnly.FromDateTime(DateTime.Now))
                        budget.BudgetSlut = budgetUpdateDTO.BudgetSlut;

                await _budgetRepository.SaveChangesAsync();

                return new BudgetAllResponseDTO
                {
                    BudgetId = budget.BudgetId,
                    BudgetName = budget.BudgetName,
                    SavingsGoal = budget.SavingsGoal,
                    BudgetSlut = budget.BudgetSlut
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating budget with ID {budgetId}: {ex.Message}");
                throw new InvalidOperationException("Could not update the budget.");
            }
        }

        // Delete a budget
        public async Task DeleteBudgetAsync(int budgetId, string userId)
        {
            if (budgetId <= 0)
                throw new ArgumentException(nameof(budgetId), "Invalid budget ID.");

            var budget = await _budgetRepository.GetByIdAsync(budgetId);
            if (budget == null || budget.BrugerId != userId) 
                throw new KeyNotFoundException($"Budget with ID {budgetId} not found or does not belong to user");

            try
            {
                await _budgetRepository.Delete(budget.BudgetId);
                await _budgetRepository.SaveChangesAsync();

                _logger.LogInformation($"Successfully deleted budget with ID {budgetId} for user");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting budget with ID {budgetId}: {ex.Message}");
                throw new InvalidOperationException("Could not delete the budget.");
            }
        }

        public decimal CalculateMonthlySavings(int savingsGoal, DateOnly budgetStart, DateOnly budgetSlut, decimal moneySaved)
        {
            if (budgetSlut <= budgetStart)
            {
                throw new ArgumentException("Budget end date must be after start date.");
            }
            //Find the monthly difference   
            int monthsToSave = ((budgetSlut.Year - budgetStart.Year) * 12) + budgetSlut.Month - budgetStart.Month;
           
           //Money already saved
           decimal results = savingsGoal - moneySaved;

            //Check if there are months to save 
            if (monthsToSave > 0)
            {
                //calculate monthly savings
                return results / monthsToSave;
            }

            return 0;
        }

        public async Task<decimal> CalculateMoneySaved(int budgetId)
        {

            // Hent udgifterne for opsparingen
            var fudgifter = await _savingRepository.GetAllByBudgetIdAsync(budgetId);

            // Beregn summen af opsparingsbeløbet
            return fudgifter.Sum(f => f.Amount);
        }
    }
}
