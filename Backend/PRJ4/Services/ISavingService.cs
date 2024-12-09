using PRJ4.DTOs;

using Microsoft.AspNetCore.Mvc;


namespace PRJ4.Services
{
    public interface ISavingService
    {
        Task<SavingResponsDTO> AddSavingAsync(int budgetId, string userId, SavingCreateDTO savingDTO);
        Task<IEnumerable<SavingResponsDTO>> GetAllSavingsAsync(int budgetId, string userId);
        Task<SavingResponsDTO> GetSavingByIdAsync(int savingId, string userId);
        Task<SavingResponsDTO> UpdateSavingAsync(int savingId, string userId, SavingUpdateDTO savingUpdateDTO);
        Task DeleteSavingAsync(int savingId, string userId);
        // Task<VudgifterResponseDTO> AddSavingAsync(int budgetId, string user, BudgetSavingCreateDTO savingDTO);
        // Task<List<BudgetSavingResponsDTO>> GetAllSavingsAsync(int budgetId, string userId);
    }
}