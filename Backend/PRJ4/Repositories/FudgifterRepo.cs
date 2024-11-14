using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.Data;
using PRJ4.Models;


namespace PRJ4.Repositories
{
    
    public class FudgifterRepo : TemplateRepo<Fudgifter>,IFudgifter
    {
        private readonly ApplicationDbContext _context;
        public FudgifterRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fudgifter>> GetAllAsync()
        {
            var Fudgifter =     await _context.Fudgifters
                .Include(f => f.Kategori)
                .ToListAsync();
            return Fudgifter;
        }

        public async Task<IEnumerable<Fudgifter>> GetAllByUserId(int brugerId)
        {
            return await _context.Fudgifters
                .Where(f => f.BrugerId == brugerId)
                .Include(f => f.Kategori)
                .ToListAsync();
        }

    }
}