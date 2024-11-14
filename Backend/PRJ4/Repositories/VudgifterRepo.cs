using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.Data;
using PRJ4.Models;


namespace PRJ4.Repositories
{
    
    public class VudgifterRepo : TemplateRepo<Vudgifter>,IVudgifter
    {
        private readonly ApplicationDbContext _context;
        public VudgifterRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vudgifter>> GetAllAsync()
        {
            var Vudgifter = await _context.Vudgifters
                .Include(f => f.Kategori)
                .ToListAsync();
            return Vudgifter;
        }
        public async Task<IEnumerable<Vudgifter>> GetAllByUserId(int brugerId)
        {
            return await _context.Vudgifters
                .Where(v => v.BrugerId == brugerId)
                .Include(v => v.Kategori)
                .ToListAsync();
        }

    }
}