using PRJ4.DTOs;
using PRJ4.Models;


namespace PRJ4.Services
{
    public interface IBudgetGoalService
    {
        Task<List<BudgetResponseDTO>> GetAllBudgetGoalsAsync();
        Task<BudgetResponseDTO> GetByIdBudgetGoalAsync(int id);
        Task<List<BudgetResponseDTO>> GetByUserIdBudgetGoalAsync(string userId);
        Task<BudgetCreateDTO> AddBudgetGoalAsync(string brugerId, BudgetCreateDTO budgetDTO);
        Task<BudgetCreateDTO> UpdateBudgetGoalAsync(int id, BudgetCreateDTO budgetDTO);
        Task<BudgetCreateDTO> DeleteBudgetAsync(int id);
    }
}