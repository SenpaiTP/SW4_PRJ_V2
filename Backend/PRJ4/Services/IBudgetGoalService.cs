using PRJ4.DTOs;

using Microsoft.AspNetCore.Mvc;


namespace PRJ4.Services
{
    public interface IBudgetGoalService
    {
        Task<List<BudgetGetbyIdDTO>> GetAllBudgetGoalsAsync();
        Task<BudgetGetbyIdDTO> GetByIdBudgetGoalAsync(int id, string userId); //Hent bestemt budget for user
        Task<List<BudgetGetAllDTO>> GetAllByUserIdBudgetGoalAsync(string userId); //Hent alle for bruger
        Task<BudgetCreateUpdateDTO> AddBudgetGoalAsync(string brugerId, BudgetCreateUpdateDTO budgetDTO);
        Task<VudgifterResponseDTO> AddSavingAsync(int budgetId, string user, SavingDTO savingDTO);
        Task<List<SavingDTO>> GetAllSavingsAsync(int budgetId, string userId);
        Task<BudgetCreateUpdateDTO> UpdateBudgetGoalAsync(int id, BudgetCreateUpdateDTO budgetDTO);
        Task DeleteBudgetAsync(int budgetId);
    }
}