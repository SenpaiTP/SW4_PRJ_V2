using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;

namespace PRJ4.Services
{
    public class BudgetGoalService: IBudgetGoalService
    {
        private readonly IBudgetRepo _budgetRepository;
        //private readonly IBrugerRepo _brugerRepository;


        public BudgetGoalService(IBudgetRepo budgetRepository)
        {
            _budgetRepository = budgetRepository;
            //_brugerRepository = brugerRepository;

        }

        public async Task<List<BudgetResponseDTO>> GetAllBudgetGoalsAsync()
        {
            var budgetListe = await _budgetRepository.GetAllAsync();
        
            if (budgetListe == null || !budgetListe.Any())
            {
                throw new KeyNotFoundException($"No budgets found.");
            }

            var budgetReturnListe = new List<BudgetResponseDTO>();

            foreach (var budget in budgetListe)
            {
                int monthlysaving = CalculateMonthlySavings(budget.SavingsGoal, budget.BudgetStart, budget.BudgetSlut);
                //decimal moneysaved = await CalculateMoneySaved(budget.BudgetId,budget.BudgetName);

                var budgetReturn = new BudgetResponseDTO
                {
                    BrugerId = budget.BrugerId,
                    BudgetId = budget.BudgetId,
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

        public async Task<BudgetResponseDTO> GetByIdBudgetGoalAsync(int id)
        {
            var budget = await _budgetRepository.GetByIdAsync(id); 
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with id {id} not found.");
            }

            int monthlysaving = CalculateMonthlySavings(budget.SavingsGoal,budget.BudgetStart,budget.BudgetSlut);
            //decimal moneysaved = await CalculateMoneySaved(budget.BudgetId,budget.BudgetName);

            var budgetReturn = new BudgetResponseDTO
            {
                BrugerId = budget.BrugerId,
                BudgetId = budget.BudgetId,
                BudgetName = budget.BudgetName,
                SavingsGoal = budget.SavingsGoal,
                BudgetSlut = budget.BudgetSlut,
                MonthlySavingsAmount = monthlysaving,
                MoneySaved = 200
            };

            return budgetReturn;
        }

        public async Task<List<BudgetResponseDTO>> GetByUserIdBudgetGoalAsync(string userId)
        {
            var budgetListe = await _budgetRepository.GetBudgetsForUserAsync(userId);
            if (budgetListe == null || !budgetListe.Any())
            {
                throw new Exception($"No budgets found for user with ID {userId}");
            }

            var budgetReturnListe = new List<BudgetResponseDTO>();

            foreach (var budget in budgetListe)
            {
                int monthlysaving = CalculateMonthlySavings(budget.SavingsGoal, budget.BudgetStart, budget.BudgetSlut);
                //decimal moneysaved = await CalculateMoneySaved(budget.BudgetId,budget.BudgetName);

                var budgetReturn = new BudgetResponseDTO
                {
                    BrugerId = budget.BrugerId,
                    BudgetId = budget.BudgetId,
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


        public async Task<BudgetCreateDTO> AddBudgetGoalAsync(string brugerId, BudgetCreateDTO budgetDTO)
        {
            //Check if user exists
            //var bruger = await _brugerRepository.GetByIdAsync(brugerId);
            
           // if(bruger == null) 
           // {throw new ArgumentException($"Bruger med id {brugerId} findes ikke. ");}
            
            //Check budgetDTO
            if (budgetDTO == null) {throw new ArgumentException($"Budget med id {budgetDTO} findes ikke. ");}
            
            //New budget
            var budget = new Budget
            {
                BrugerId = brugerId,
                BudgetName = budgetDTO.BudgetName,
                SavingsGoal = budgetDTO.SavingsGoal,
                BudgetStart =  DateOnly.FromDateTime(DateTime.Now),
                BudgetSlut = budgetDTO.BudgetSlut
            };

            //Add and save the budget
            var createdBudget = await _budgetRepository.AddAsync(budget);
            await _budgetRepository.SaveChangesAsync();


            // Map det oprettede budget til BudgetCreateDTO
            var createdBudgetDTO = new BudgetCreateDTO
            {
                BudgetName = createdBudget.BudgetName,
                SavingsGoal = createdBudget.SavingsGoal,
                BudgetSlut = createdBudget.BudgetSlut
            };

            //Return created budget
            return createdBudgetDTO;

        }

        public async Task<BudgetCreateDTO> UpdateBudgetGoalAsync(int id, BudgetCreateDTO budgetDTO)
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

            existingBudget.BudgetName =budgetDTO.BudgetName;
            existingBudget.SavingsGoal = budgetDTO.SavingsGoal;
            existingBudget.BudgetSlut = budgetDTO.BudgetSlut;
            
            try
            {
                await _budgetRepository.Update(existingBudget); 
                await _budgetRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"HEYYY: Error updating budget: {ex.Message}");
                return null;
            }

            var updatedBudgetDTO = new BudgetCreateDTO
            {
                BudgetName = existingBudget.BudgetName,
                SavingsGoal = existingBudget.SavingsGoal,
                BudgetSlut = existingBudget.BudgetSlut
            };
            

            return updatedBudgetDTO;

        }

        public async Task<BudgetCreateDTO> DeleteBudgetAsync(int id)
        {
            var budget = await _budgetRepository.GetByIdAsync(id);
            if (budget == null)
            {
                throw new KeyNotFoundException($"Budget with id {id} not found.");
            }

            var deletedBudget = await _budgetRepository.Delete(id);
            await _budgetRepository.SaveChangesAsync();

            var updatedBudgetDTO = new BudgetCreateDTO
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

        // public async Task<decimal> CalculateMoneySaved(int brugerId, string savingName)
        // {
        //     // Hent udgifterne for opsparingen
        //     var fudgifter = await _budgetRepository.GetExspencesByKategori(brugerId, savingName);

        //     // Beregn summen af opsparingsbeløbet
        //     return fudgifter.Sum(f => f.Pris);
        // }




    }
}