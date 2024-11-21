using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.Data;
using PRJ4.Models;
using PRJ4.DTOs;

namespace PRJ4.Repositories
{
    public class FindtægtRepo : TemplateRepo<Findtægt>, IFindtægtRepo
    {
        private readonly ApplicationDbContext _context;
        public FindtægtRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Findtægt>> GetAllAsync()
        {
            var Findtægt = await _context.Findtægter
                //.Include(f => f.Kategori)
                .ToListAsync();
            return Findtægt;
        }
        public async Task<IEnumerable<Findtægt>> GetAllByUserId(int brugerId)
        {
            return await _context.Findtægter
                .Where(v => v.BrugerId == brugerId)
                //.Include(v => v.Kategori)
                .ToListAsync();
        }

        public async Task<IEnumerable<Findtægt>> GetByUserIdAsync(int userId)
        {
            return await _context.Findtægter.Where(f => f.BrugerId == userId).ToListAsync();
        }

        public async Task UpdateAsync(Findtægt findtægt)
        {
            _context.Findtægter.Update(findtægt);
            await _context.SaveChangesAsync();
        }

    }
}