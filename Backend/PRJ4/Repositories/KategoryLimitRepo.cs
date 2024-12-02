using PRJ4.Data;
using PRJ4.Models; 
using Microsoft.EntityFrameworkCore;

namespace PRJ4.Repositories;

public class KategoryLimitRepo: TemplateRepo<KategoryLimit>, IKategoryLimitRepo
{
    private readonly ApplicationDbContext _context;
    public KategoryLimitRepo(ApplicationDbContext context) : base(context) 
    {
        _context = context;
    }

    public async Task<List<KategoryLimit>> GetBudgetKategoriesForUserAsync(string userId)
    {
        return await _context.KategoryLimits
            .Where(b => b.BrugerId == userId)
            .ToListAsync();
    }
    
}