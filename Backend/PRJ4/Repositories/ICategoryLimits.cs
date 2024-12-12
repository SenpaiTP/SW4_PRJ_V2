using PRJ4.Models;
namespace PRJ4.Repositories
{
    public interface ICategoryLimitRepo:ITemplateRepo<CategoryLimit>
    {
        Task<List<CategoryLimit>> GetCategoryLimitsForUserAsync(string userId);
        Task<CategoryLimit?> GetCategoryLimitForCategoryAsync(int CategoryId, string userId);
        
    }
}