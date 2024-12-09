using PRJ4.Data;
using PRJ4.Models; 
using Microsoft.EntityFrameworkCore;

namespace PRJ4.Repositories;

public class SavingRepo: TemplateRepo<Saving>, ISavingRepo
{
    private readonly ApplicationDbContext _context;
    public SavingRepo(ApplicationDbContext context) : base(context) 
    {
        _context = context;
    }

    public async Task<IEnumerable<Saving>> GetAllByBudgetIdAsync(int budgetId)
    {

            // Fetch all savings related to the specified budgetId
        var savings = await _context.Savings
            .Where(s => s.BudgetId == budgetId)
            .ToListAsync();

        return savings;
      
    }

}