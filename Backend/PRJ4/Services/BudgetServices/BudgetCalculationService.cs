using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;



namespace PRJ4.Services.BudgetService
{
    public class BudgetCalculationService
    {
    

        public BudgetCalculationService()
        {
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