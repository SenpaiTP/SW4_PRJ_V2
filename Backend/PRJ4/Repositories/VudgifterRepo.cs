using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.Data;
using PRJ4.Models;


namespace PRJ4.Repositories
{
    
    public class VudgifterRepo : TemplateRepo<Vudgifter>,IVudgifterRepo
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
        public async Task<IEnumerable<Vudgifter>> GetAllByUserId(string brugerId)
        {
            return await _context.Vudgifters
                .Where(v => v.BrugerId == brugerId)
                .Include(v => v.Kategori)
                .ToListAsync();
        }

         public async Task<IEnumerable<Vudgifter>> GetAllByCategory(string brugerId, int kategoryId)
        {

            return await _context.Vudgifters
            .Where(v => v.KategoriId == kategoryId && v.BrugerId == brugerId)
            .Include(v => v.Kategori) // Including the Kategori for reference
            .ToListAsync();
        }
        public async Task<IEnumerable<Vudgifter>> GetAllByDate(string brugerId, DateTime from, DateTime end)
        {
            return await _context.Vudgifters
                .Where(v => v.BrugerId == brugerId && v.Dato >= from && v.Dato <= end)
                .Include(v => v.Kategori) // Including the Kategori for reference
                .ToListAsync();
        }

        public async Task<IEnumerable<Vudgifter>> GetAllByCategoryADate(string brugerId, int kategoryId,DateTime from, DateTime end)
        {
            return await _context.Vudgifters
                .Where(v => v.BrugerId == brugerId && v.Dato >= from && v.Dato <= end && v.KategoriId == kategoryId)
                .Include(v => v.Kategori) // Including the Kategori for reference
                .ToListAsync();
        }

    }
}