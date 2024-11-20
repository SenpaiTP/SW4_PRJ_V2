using PRJ4.DTOs;
using System.Threading.Tasks;

namespace PRJ4.Services.BudgetService
{
    public interface IBudgetAmountService
    {
        Task<List<BudgetGetDTO>> GetAllBudgetsWithMonthlySavingsAsync();
        Task<BudgetGetDTO> GetByIdBudgetWithMonthlySavingsAsync(int id);
    }
}