using PRJ4.Models;
namespace PRJ4.Repositories
{
    public interface IKategoryLimitRepo:ITemplateRepo<KategoryLimit>
    {
        Task<List<KategoryLimit>> GetBudgetKategoriesForUserAsync(string userId);
        
    }
}