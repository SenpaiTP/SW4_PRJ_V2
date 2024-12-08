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

    public async Task<Budget> GetByIdWithKategoriAsync(int id)
    {
        return await _context.Budgets
            .Include(b => b.Kategory)  // Include Kategori-relationen
            .FirstOrDefaultAsync(b => b.BudgetId == id);  // Find budget med id
    } 
}