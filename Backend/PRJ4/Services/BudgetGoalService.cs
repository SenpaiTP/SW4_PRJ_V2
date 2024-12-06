using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace PRJ4.Services
{
    public class BudgetGoalService: IBudgetGoalService
    {
        private readonly IBudgetRepo _budgetRepository;
        private readonly IKategoriRepo _kategoryRepositry;
        private readonly IVudgifterService _vudgifterService;
        private readonly IVudgifterRepo _vudgifterRepository;


        public BudgetGoalService(IBudgetRepo budgetRepository, IKategoriRepo kategoryRepositry, IVudgifterService vudgifterService, IVudgifterRepo vudgifterRepository)
        {
            _budgetRepository = budgetRepository;
            _kategoryRepositry = kategoryRepositry;
            _vudgifterService = vudgifterService;
            _vudgifterRepository = vudgifterRepository;

        }


        //Skal ikke bruge denne medtode
        public async Task<List<BudgetGetbyIdDTO>> GetAllBudgetGoalsAsync()
        {
            var budgetListe = await _budgetRepository.GetAllAsync();
        
            if (budgetListe == null || !budgetListe.Any())
            {
                throw new KeyNotFoundException($"No budgets found.");
            }

            var budgetReturnListe = new List<BudgetGetbyIdDTO>();

            foreach (var budget in budgetListe)
            {
                int monthlysaving = CalculateMonthlySavings(budget.SavingsGoal, budget.BudgetStart, budget.BudgetSlut);
                //decimal moneysaved = await CalculateMoneySaved(budget.BudgetId,budget.BudgetName);

                var budgetReturn = new BudgetGetbyIdDTO
                {
                    BudgetId = budget.BudgetId,
                    KategoryId = budget.KategoryId,
                    BudgetName = budget.BudgetName,
                    SavingsGoal = budget.SavingsGoal,
                    BudgetSlut = budget.BudgetSlut,
                    MonthlySavingsAmount = monthlysaving,
                    MoneySaved = 200
                };

                budgetReturnListe.Add(budgetReturn);
            }

            return budgetReturnListe;
        }

        public async Task<BudgetGetbyIdDTO> GetByIdBudgetGoalAsync(int budgetId, string userId)
        {
            //Validating parameter
            if (budgetId <= 0)
            {
                throw new ArgumentException("Invalid budget ID.", nameof(budgetId));
            }

            // Get budget from repository
            var budget = await _budgetRepository.GetByIdAsync(budgetId); 
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with id {budgetId} not found.");
            }

            //Calculate monthly savings and money saved
            int monthlysaving = CalculateMonthlySavings(budget.SavingsGoal,budget.BudgetStart,budget.BudgetSlut);
            decimal savingResult = await CalculateSavingsForCategoryAsync(userId, budget.KategoryId);

            // Return DTO 
            var budgetReturn = new BudgetGetbyIdDTO
            {
                BudgetId = budget.BudgetId,
                BudgetName = budget.BudgetName,
                KategoryId = budget.KategoryId,
                SavingsGoal = budget.SavingsGoal,
                BudgetSlut = budget.BudgetSlut,
                MonthlySavingsAmount = monthlysaving,
                MoneySaved = savingResult
            };

            return budgetReturn;
        }

        public async Task<List<SavingDTO>> GetAllSavingsAsync(int budgetId, string userId)
        {
            // Validate parameter
            if (budgetId <= 0)
            {
                throw new ArgumentException("Invalid budget ID.", nameof(budgetId));
            }
            var budget = await GetByIdBudgetGoalAsync(budgetId, userId);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with ID {budgetId} not found.");
            }

            var udgifter = await _vudgifterRepository.GetAllByCategory(userId, budget.KategoryId);
            var udgiftReturnListe = new List<SavingDTO>();

            foreach (var udgift in udgifter)
            {
                var saving = new SavingDTO
                {
                    Saving = udgift.Pris,
                    Date = udgift.Dato
                };
                udgiftReturnListe.Add(saving);

            }

            return udgiftReturnListe;

        }

        public async Task<List<BudgetGetAllDTO>> GetAllByUserIdBudgetGoalAsync(string userId)
        {
            var budgetListe = await _budgetRepository.GetBudgetsForUserAsync(userId);
            if (budgetListe == null || !budgetListe.Any())
            {
                throw new Exception($"No budgets found for user with ID {userId}");
            }

            var budgetReturnListe = new List<BudgetGetAllDTO>();

            foreach (var budget in budgetListe)
            {
                
                var budgetReturn = new BudgetGetAllDTO
                {
                    BudgetId = budget.BudgetId,
                    KategoryId = budget.KategoryId,
                    BudgetName = budget.BudgetName,
                    SavingsGoal = budget.SavingsGoal,
                    BudgetSlut = budget.BudgetSlut,
                };

                budgetReturnListe.Add(budgetReturn);
            }

            return budgetReturnListe;


        }


        public async Task<BudgetCreateUpdateDTO> AddBudgetGoalAsync(string brugerId, BudgetCreateUpdateDTO budgetDTO)
        {
            // Validate parameters
            if (budgetDTO == null)
            {
                throw new ArgumentNullException(nameof(budgetDTO), "Budget data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(budgetDTO.BudgetName) || budgetDTO.BudgetSlut == default)
            {
                throw new ArgumentException("Budget name and end date must be provided.");
            }

            // Validate enddate 
            var today = DateOnly.FromDateTime(DateTime.Now);
            if (budgetDTO.BudgetSlut <= today)
            {
                throw new ArgumentException("Budget end date must be in the future.");
            }
            if (budgetDTO.BudgetSlut > today.AddYears(10))
            {
                throw new ArgumentException("Budget end date cannot be more than 10 years from today.");
            }


            // Validate savingGoal
            if (budgetDTO.SavingsGoal <= 0)
            {
                throw new ArgumentException("Savings goal must be greater than zero.");
            }

            if (budgetDTO.SavingsGoal > 10_000_000)
            {
                throw new ArgumentException("Savings goal cannot exceed 10,000,000.");
            }

        

            //New kategory
            var kategoryName = $"Opsparing: {budgetDTO.BudgetName}";
            var kategori = await _kategoryRepositry.NyKategori(kategoryName);
            if (kategori == null)
            {
                throw new InvalidOperationException($"Failed to create or retrieve category '{kategoryName}'.");
            }
            
            //New budget
            var budget = new Budget
            {
                BudgetName = budgetDTO.BudgetName,
                KategoryId = kategori.KategoriId,
                BrugerId = brugerId,
                SavingsGoal = budgetDTO.SavingsGoal,
                BudgetStart =  DateOnly.FromDateTime(DateTime.Now),
                BudgetSlut = budgetDTO.BudgetSlut
            };

          
            //Add and save the budget
            var createdBudget = await _budgetRepository.AddAsync(budget);
            await _budgetRepository.SaveChangesAsync();


            // Map det oprettede budget til BudgetCreateDTO
            var createdBudgetDTO = new BudgetCreateUpdateDTO
            {
                BudgetName = createdBudget.BudgetName,
                SavingsGoal = createdBudget.SavingsGoal,
                BudgetSlut = createdBudget.BudgetSlut
            };

            //Return created budget
            return createdBudgetDTO;

        }

        public async Task<VudgifterResponseDTO> AddSavingAsync(int budgetId, string user, SavingDTO savingDTO)
        {
            // Validate for parameter
            if (savingDTO == null)
            {
                throw new ArgumentNullException(nameof(savingDTO), "Saving data cannot be null.");
            }
            if (budgetId <= 0)
            {
                throw new ArgumentException("Invalid budget ID.", nameof(budgetId));
            }

            var budget = await GetByIdBudgetGoalAsync(budgetId, user);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with ID {budgetId} not found for user {user}.");
            }
            
      
            var kategory = await _kategoryRepositry.GetByIdAsync(budget.KategoryId);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with ID {budgetId} not found for user {user}.");
            }
    
            var saving = new nyVudgifterDTO
            {
                KategoriNavn = kategory.KategoriNavn,
                KategoriId = budget.KategoryId,
                Pris = savingDTO.Saving, 
                Dato = savingDTO.Date
            };
            
            var savingAdded = await _vudgifterService.AddVudgifter(user, saving); 
            await _budgetRepository.SaveChangesAsync();
            
            return savingAdded;

        }


        public async Task<BudgetCreateUpdateDTO> UpdateBudgetGoalAsync(int id, BudgetCreateUpdateDTO budgetDTO)
        {
            // Validate parameter
            if (budgetDTO == null)
            {
                throw new ArgumentException("Invalid budget data.");
            }

            //Validate budgetName
            if (string.IsNullOrWhiteSpace(budgetDTO.BudgetName))
            {
                throw new ArgumentException("Budget name cannot be empty.");
            }

            // Validate savingGoal
            if (budgetDTO.SavingsGoal <= 0)
            {
                throw new ArgumentException("Savings goal must be greater than zero.");
            }

            // Validate enddate
            if (budgetDTO.BudgetSlut <= DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ArgumentException("Budget end date must be in the future.");
            }

            var existingBudget = await _budgetRepository.GetByIdAsync(id);
            if (existingBudget == null)
            {
                throw new KeyNotFoundException($"Budget with ID {id} not found.");
            }

            existingBudget.BudgetName = budgetDTO.BudgetName;
            existingBudget.SavingsGoal = budgetDTO.SavingsGoal;
            existingBudget.BudgetSlut = budgetDTO.BudgetSlut;
            
            try
            { 
                await _budgetRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Could not update budget.", ex);
            }

            var updatedBudgetDTO = new BudgetCreateUpdateDTO
            {
                BudgetName = existingBudget.BudgetName,
                SavingsGoal = existingBudget.SavingsGoal,
                BudgetSlut = existingBudget.BudgetSlut
            };

            return updatedBudgetDTO;

        }

        public async Task DeleteBudgetAsync(int budgetId)
        {
            if (budgetId <= 0)
            {
                throw new ArgumentException("Invalid budget ID. ID must be greater than zero.");
            }

            var budget = await _budgetRepository.GetByIdAsync(budgetId);
            if (budget == null)
            {
                 throw new KeyNotFoundException($"The budget with ID {budgetId} does not exist and cannot be deleted.");
            }

            var deletedBudget = await _budgetRepository.Delete(budgetId);
            await _budgetRepository.SaveChangesAsync();
        }

//Calculations
        public int CalculateMonthlySavings(int savingsGoal, DateOnly budgetStart, DateOnly budgetSlut)
        {
            if (budgetSlut <= budgetStart)
            {
                throw new ArgumentException("Budget end date must be after start date.");
            }
            if (savingsGoal <= 0)
            {
                throw new ArgumentException("Savings goal must be greater than zero.");
            }

            // Calculate number of days form start to enddate
            var startDate = budgetStart.ToDateTime(new TimeOnly(0, 0)); // Konverter startdato til DateTime
            var endDate = budgetSlut.ToDateTime(new TimeOnly(23, 59));  // Konverter slutdato til DateTime

            var totalDays = (endDate - startDate).Days;

            // Calculate total months
            var totalMonths = (totalDays / 30.0); // Brug gennemsnit af dage pr. måned (30 dage)

            //Check if there are months to save 
            if (totalMonths > 0)
            {
                //calculate monthly savings
                return (int)(savingsGoal / totalMonths);
            }

            return 0;
        }


        private async Task<decimal> CalculateSavingsForCategoryAsync(string userId, int categoryId)
        {
            // Validate parameters
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.");
            }
            
            if (categoryId <= 0)
            {
                throw new ArgumentException("CategoryId must be greater than zero.");
            }

            var udgifter = await _vudgifterRepository.GetAllByCategory(userId, categoryId);

            decimal savingResult = 0;
            foreach (var udgift in udgifter)
            {
                savingResult += udgift.Pris;
            }

            return savingResult;
        }

      



    }
}