using PRJ4.DTOs;

using Microsoft.AspNetCore.Mvc;


namespace PRJ4.Services
{
    public interface IBudgetService
    {
        Task<BudgetResponseDTO> GetBudgetByIdAsync(int budgetId, string userId);
        Task<List<BudgetAllResponseDTO>> GetAllBudgetsAsync(string userId);
        Task<BudgetAllResponseDTO> AddBudgetAsync(string userId, BudgetCreateDTO budgetDTO);
        Task<BudgetAllResponseDTO> UpdateBudgetAsync(int budgetId, string userId, BudgetUpdateDTO budgetUpdateDTO);
        Task DeleteBudgetAsync(int budgetId, string userId);
    }
}