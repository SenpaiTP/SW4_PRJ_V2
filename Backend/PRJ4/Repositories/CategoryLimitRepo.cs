using PRJ4.Data;
using PRJ4.Models; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace PRJ4.Repositories;

public class CategoryLimitRepo: TemplateRepo<CategoryLimit>, ICategoryLimitRepo
{
    private readonly ApplicationDbContext _context;

    public CategoryLimitRepo(ApplicationDbContext context) : base(context) 
    {
        _context = context;
    }

    public async Task<List<CategoryLimit>> GetCategoryLimitsForUserAsync(string userId)
    {
        return await _context.CategoryLimits
            .Where(b => b.BrugerId == userId)
            .Include(b => b.Category)
            .ToListAsync();  
    }

    public async Task<CategoryLimit?> GetCategoryLimitForCategoryAsync(int CategoryLimitId,string userId)
    {
        return await _context.CategoryLimits
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.BrugerId == userId && b.CategoryLimitId == CategoryLimitId);  
    }   
}