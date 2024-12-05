using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace PRJ4.Services
{
    public class BudgetGoalService: IBudgetGoalService
    {
        private readonly IBudgetRepo _budgetRepository;
        private readonly IBrugerRepo _brugerRepository;
        private readonly IKategori _kategoryRepositry;
        private readonly IVudgifterService _vudgifterService;

        private readonly IVudgifter _vudgifterRepository;


        public BudgetGoalService(IBudgetRepo budgetRepository, IBrugerRepo brugerRepository, IKategori kategoryRepositry, IVudgifterService vudgifterService, IVudgifter vudgifterRepository)
        {
            _budgetRepository = budgetRepository;
            _brugerRepository = brugerRepository;
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

        public async Task<BudgetGetbyIdDTO> GetByIdBudgetGoalAsync(int id, string userId)
        {
            var budget = await _budgetRepository.GetByIdAsync(id); 
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with id {id} not found.");
            }

            int monthlysaving = CalculateMonthlySavings(budget.SavingsGoal,budget.BudgetStart,budget.BudgetSlut);
         
            decimal savingResult = await CalculateSavingsForCategoryAsync(userId, budget.KategoryId);

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
            var budget = await GetByIdBudgetGoalAsync(budgetId, userId);
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
            //Check budgetDTO
            if (budgetDTO == null) 
            {
                throw new ArgumentException($"Budget med id {budgetDTO} findes ikke. ");
            }

        

            //New kategory
            var kategoryName = $"Opsparing: {budgetDTO.BudgetName}";
            var kategori = await _kategoryRepositry.NyKategori(kategoryName);
            
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

        public async Task<VudgifterResponseDTO> AddSavingAsync(string user, SavingDTO savingDTO, int budgetId)
        {
            var budget = await GetByIdBudgetGoalAsync(budgetId, user);
      
            var kategory = await _kategoryRepositry.GetByIdAsync(budget.KategoryId);
    
            var saving = new nyVudgifterDTO
            {
                KategoriNavn = kategory.Navn,
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
            if (budgetDTO == null)
            {
                throw new ArgumentException("Invalid budget data.");
            }

            var existingBudget = await _budgetRepository.GetByIdAsync(id);
            if (existingBudget == null)
            {
                throw new ArgumentException($"Budget with id {id} not found.");
            }

            existingBudget.BudgetName = budgetDTO.BudgetName;
            existingBudget.SavingsGoal = budgetDTO.SavingsGoal;
            existingBudget.BudgetSlut = budgetDTO.BudgetSlut;
            
            try
            {
                await _budgetRepository.Update(existingBudget); 
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

        public async Task<BudgetCreateUpdateDTO> DeleteBudgetAsync(int id)
        {
            var budget = await _budgetRepository.GetByIdAsync(id);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with id {id} not found.");
            }

            var deletedBudget = await _budgetRepository.Delete(id);
            await _budgetRepository.SaveChangesAsync();

            var updatedBudgetDTO = new BudgetCreateUpdateDTO
            {
                BudgetName = deletedBudget.BudgetName,
                SavingsGoal = deletedBudget.SavingsGoal,
                BudgetSlut = deletedBudget.BudgetSlut
            };

            return updatedBudgetDTO;
        }

//Calculations
        public int CalculateMonthlySavings(int savingsGoal, DateOnly budgetStart, DateOnly budgetSlut)
        {
            if (budgetSlut <= budgetStart)
            {
                throw new ArgumentException("Budget end date must be after start date.");
            }
            //Find the monthly difference   
            int monthsToSave = ((budgetSlut.Year - budgetStart.Year) * 12) + budgetSlut.Month - budgetStart.Month;
           
            //Check if there are months to save 
            if (monthsToSave > 0)
            {
                //calculate monthly savings
                return savingsGoal / monthsToSave;
            }

            return 0;
        }


        private async Task<decimal> CalculateSavingsForCategoryAsync(string userId, int categoryId)
        {
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