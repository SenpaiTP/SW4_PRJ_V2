using PRJ4.DTOs;

using Microsoft.AspNetCore.Mvc;


namespace PRJ4.Services
{
    public interface IBudgetGoalService
    {
        Task<BudgetResponsDTO> GetByIdBudgetGoalAsync(int budgetId, string userId); //Hent bestemt budget for user
        Task<List<BudgetResponsDTO>> GetAllByUserIdBudgetGoalAsync(string userId); //Hent alle for bruger
        Task<BudgetCreateDTO> AddBudgetGoalAsync(string brugerId, BudgetCreateDTO budgetDTO);
        Task<VudgifterResponseDTO> AddSavingAsync(int budgetId, string user, BudgetSavingCreateDTO savingDTO);
        Task<List<BudgetSavingResponsDTO>> GetAllSavingsAsync(int budgetId, string userId);
        Task<BudgetUpdateDTO> UpdateBudgetGoalAsync(int id, BudgetUpdateDTO budgetDTO);
        Task DeleteBudgetAsync(int budgetId);
    }
}