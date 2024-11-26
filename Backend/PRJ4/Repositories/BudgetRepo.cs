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

    public async Task<List<Budget>> GetBudgetsForUserAsync(int userId)
    {
        return await _context.Budgets
            .Where(b => b.BrugerId == userId)
            .ToListAsync();
    }

    public async Task<List<Fudgifter>> GetExspencesByKategori(int brugerId, string savingName)
    {
        // Hent alle udgifter for en bruger med den specifikke kategori
        return await _context.Fudgifters
            .Where(f => f.BrugerId == brugerId && f.Kategori.Name == savingName)
            .ToListAsync();
    }
}
