using PRJ4.DTOs;

using Microsoft.AspNetCore.Mvc;


namespace PRJ4.Services
{
    public interface IBudgetGoalService
    {
        Task<BudgetResponseDTO> GetBudgetByIdAsync(int budgetId, string userId);
        Task<List<BudgetResponseDTO>> GetAllBudgetsAsync(string userId);
        Task<BudgetResponseDTO> AddBudgetAsync(string userId, BudgetCreateDTO budgetDTO);
        Task<BudgetResponseDTO> UpdateBudgetAsync(int budgetId, string userId, BudgetUpdateDTO budgetUpdateDTO);
        Task DeleteBudgetAsync(int budgetId, string userId);
    }
}