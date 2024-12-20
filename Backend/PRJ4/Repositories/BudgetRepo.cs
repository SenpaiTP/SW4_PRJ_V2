using PRJ4.Data;
using PRJ4.Models; 
using Microsoft.EntityFrameworkCore;

namespace PRJ4.Repositories;

public class BudgetRepo: TemplateRepo<Budget>, IBudgetRepo
{
    private readonly ApplicationDbContext _context;
    public BudgetRepo(ApplicationDbContext context) : base(context) 
    {
        _context = context;
    }

    public async Task<List<Budget>> GetBudgetsForUserAsync(string userId)
    {
        return await _context.Budgets
            .Where(b => b.BrugerId == userId)
            .ToListAsync();
    } 
}