using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace PRJ4.Services
{
    public class SavingService: ISavingService
    {
        private readonly ISavingRepo _savingRepository;
        private readonly IBudgetRepo _budgetRepository;
        private readonly ILogger<SavingService> _logger;

        public SavingService(ISavingRepo savingRepository,
                            IBudgetRepo budgetRepository, 
                            ILogger<SavingService> logger)
        {
            _savingRepository = savingRepository;
            _budgetRepository = budgetRepository;
            _logger = logger;
        }
        public async Task<SavingResponsDTO> AddSavingAsync(int budgetId, string userId, SavingCreateDTO savingDTO)
        {
            // Validate input parameters
            if (savingDTO == null)
            {
                throw new ArgumentException(nameof(savingDTO), "Saving data cannot be null.");
            }
            if (savingDTO.Amount < 0)
            {
                throw new ArgumentException(nameof(savingDTO), "Saving has to be positive");
            }
            if (budgetId <= 0)
            {
                throw new ArgumentException(nameof(budgetId), "Invalid budget ID.");
            }

            // Get the budget by its ID
            var budget = await _budgetRepository.GetByIdAsync(budgetId);
            if (budget == null || budget.BrugerId != userId)  
            {
                 throw new KeyNotFoundException($"Budget with ID {budgetId} not found or does not belong to user");
            }
         

            // Create a new Saving object based on the input data
            var saving = new Saving
            {
                Amount = savingDTO.Amount,
                Date = savingDTO.Date,
                BudgetId = budgetId
            };

            try
            {
                // Add the saving to the budget's collection of savings
                var createdBudget = await _savingRepository.AddAsync(saving);

                // Save changes to the database
                await _savingRepository.SaveChangesAsync();

                // Log success
                _logger.LogInformation($"Successfully added saving of {saving.Amount} to budget ID: {budgetId} for user ID: {userId}.");

                return  new SavingResponsDTO
                {
                    savingId = createdBudget.SavingId,
                    Amount = createdBudget.Amount,
                    Date = createdBudget.Date
                };
            }
            catch (Exception ex)
            {
                // Log error if something goes wrong
                _logger.LogError($"Error while saving the new saving: {ex.Message}");
                throw new InvalidOperationException("Could not save the new saving.");
            }
        }

        public async Task<IEnumerable<SavingResponsDTO>> GetAllSavingsAsync(int budgetId, string userId)
        {
            // Get the budget by its ID
            var budget = await _budgetRepository.GetByIdAsync(budgetId);
            if (budget == null || budget.BrugerId != userId)  // Ensure the budget belongs to the correct user
            {
                throw new KeyNotFoundException($"Budget with ID {budgetId} not found or does not belong to user.");
            }

            try
            {
                // Get all savings for the budget
                var savings = await _savingRepository.GetAllByBudgetIdAsync(budgetId);

                // Log success
                _logger.LogInformation($"Successfully retrieved {savings.Count()} savings for budget ID: {budgetId} for user ID: {userId}.");

                // Return the list of savings as DTOs
                return savings.Select(s => new SavingResponsDTO
                {
                    savingId = s.SavingId,
                    Amount = s.Amount,
                    Date = s.Date
                });
            }
            catch (Exception ex)
            {
                // Log error if something goes wrong
                _logger.LogError($"Error while retrieving all savings: {ex.Message}");
                throw new InvalidOperationException("Could not retrieve savings.");
            }
        }

        public async Task<SavingResponsDTO> GetSavingByIdAsync(int savingId, string userId)
        {
            try
            {
                // Get the saving by its ID for the specified budget
                var saving = await _savingRepository.GetByIdAsync(savingId);
                if (saving == null)
                {
                    throw new KeyNotFoundException($"Saving with ID {savingId} not found.");
                }

                // Log success
                _logger.LogInformation($"Successfully retrieved saving with ID: {savingId} for user ID: {userId}.");

                // Return the saving as a DTO
                return new SavingResponsDTO
                {
                    savingId = saving.SavingId,
                    Amount = saving.Amount,
                    Date = saving.Date
                };
            }
            catch (Exception ex)
            {
                // Log error if something goes wrong
                _logger.LogError($"Error while retrieving saving with ID {savingId}: {ex.Message}");
                throw new InvalidOperationException("Could not retrieve the saving.");
            }
        }

        public async Task<SavingResponsDTO> UpdateSavingAsync(int savingId, string userId, SavingUpdateDTO savingUpdateDTO)
        {
            // Validate input parameters
            if (savingUpdateDTO == null)
            {
                throw new ArgumentException(nameof(savingUpdateDTO), "Saving data cannot be null.");
            }

            try
            {
                // Get the saving by its ID for the specifik budget
                var saving = await _savingRepository.GetByIdAsync(savingId);
                if (saving == null )
                {
                    throw new KeyNotFoundException($"Saving with ID {savingId} not found.");
                }
                if (saving.BudgetId <= 0)
                {
                    throw new ArgumentException(nameof(saving.BudgetId), "Invalid budget ID.");
                }
              
                // Update the saving
                saving.Amount = savingUpdateDTO.Amount;
                saving.Date = savingUpdateDTO.Date;

                // Save changes to the database
                await _savingRepository.SaveChangesAsync();

                // Log success
                _logger.LogInformation($"Successfully updated saving with ID: {savingId}for user ID: {userId}.");

                // Return the updated saving as a DTO
                return new SavingResponsDTO
                {
                    savingId = saving.SavingId,
                    Amount = saving.Amount,
                    Date = saving.Date
                };
            }
            catch (Exception ex)
            {
                // Log error if something goes wrong
                _logger.LogError($"Error while updating saving with ID {savingId}: {ex.Message}");
                throw new InvalidOperationException("Could not update the saving.");
            }
        }


        public async Task DeleteSavingAsync(int savingId, string userId)
        {
           
            try
            {
                // Retrieve the saving by its ID for the specified budget
                var saving = await _savingRepository.GetByIdAsync(savingId);
                if (saving == null)
                {
                    throw new KeyNotFoundException($"Saving with ID {savingId} not found");
                }

          
                // Delete the saving from the repository
                await _savingRepository.Delete(savingId);
                await _savingRepository.SaveChangesAsync();

                // Log success
                _logger.LogInformation($"Successfully deleted saving with ID: {savingId} for user ID: {userId}.");
            }
            catch (Exception ex)
            {
                // Log error if something goes wrong
                _logger.LogError($"Error while deleting saving with ID {savingId}: {ex.Message}");
                throw new InvalidOperationException("Could not delete the saving.");
            }
        }
       
    }

}