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
            var Fudgifter = await _context.Fudgifters
                .Include(f => f.Kategori)
                .ToListAsync();
            return Fudgifter;
        }

        public async Task<IEnumerable<Fudgifter>> GetAllByUserId(string brugerId)
        {
            return await _context.Fudgifters
                .Where(f => f.BrugerId == brugerId)
                .Include(f => f.Kategori)
                .ToListAsync();
        }

        public async Task<IEnumerable<Fudgifter>> GetAllByCategory(string brugerId, int kategoryId)
        {

            return await _context.Fudgifters
            .Where(f => f.KategoriId == kategoryId && f.BrugerId == brugerId)
            .Include(f => f.Kategori) // Including the Kategori for reference
            .ToListAsync();
        }
        public async Task<IEnumerable<Fudgifter>> GetAllByDate(string brugerId, DateTime from, DateTime end)
        {
            return await _context.Fudgifters
                .Where(f => f.BrugerId == brugerId && f.Dato >= from && f.Dato <= end)
                .Include(f => f.Kategori) // Including the Kategori for reference
                .ToListAsync();
        }

        public async Task<IEnumerable<Fudgifter>> GetAllByCategoryADate(string brugerId, int kategoryId,DateTime from, DateTime end)
        {
            return await _context.Fudgifters
                .Where(f => f.BrugerId == brugerId && f.Dato >= from && f.Dato <= end && f.KategoriId == kategoryId)
                .Include(f => f.Kategori) // Including the Kategori for reference
                .ToListAsync();
        }


    }
}