using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;



namespace PRJ4.Services.BudgetService
{
    public class BudgetAmountService: IBudgetAmountService
    {
        private readonly IBudgetRepo _budgetRepository;


        public BudgetAmountService(IBudgetRepo budgetRepository)
        {
            _budgetRepository = budgetRepository;

        }

        public async Task<List<BudgetGetDTO>> GetAllBudgetsWithMonthlySavingsAsync()
        {
            var budgetListe = await _budgetRepository.GetAllAsync();
        
            if (budgetListe == null || !budgetListe.Any())
            {
                return null; // Returner null eller en tom liste, hvis der ikke findes budgets
            }

            var budgetReturnListe = new List<BudgetGetDTO>();

            foreach (var budget in budgetListe)
            {
                int monthlysaving = CalculateMonthlySavings(budget.SavingsGoal, budget.BudgetStart, budget.BudgetSlut);

                var budgetReturn = new BudgetGetDTO
                {
                    BudgetId = budget.BudgetId,
                    SavingsGoal = budget.SavingsGoal,
                    BudgetStart = budget.BudgetStart,
                    BudgetSlut = budget.BudgetSlut,
                    MonthlySavingsAmount = monthlysaving
                };

                budgetReturnListe.Add(budgetReturn);
            }

            return budgetReturnListe;
        }

        public async Task<BudgetGetDTO> GetByIdBudgetWithMonthlySavingsAsync(int id)
        {
            var budget = await _budgetRepository.GetByIdAsync(id); 
            if (budget == null)
            {
                return null; 
            }

            int monthlysaving = CalculateMonthlySavings(budget.SavingsGoal,budget.BudgetStart,budget.BudgetSlut);

            var budgetReturn = new BudgetGetDTO
            {
                SavingsGoal = budget.SavingsGoal,
                BudgetStart = budget.BudgetStart,
                BudgetSlut = budget.BudgetSlut,
                MonthlySavingsAmount = monthlysaving
            };

            return budgetReturn;
        }

        public int CalculateMonthlySavings(int savingsGoal, DateOnly budgetStart, DateOnly budgetSlut)
        {
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

    }
}