
using Microsoft.EntityFrameworkCore;
using PRJ4.Data;
using PRJ4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRJ4.Repositories
{
    public class KategoriRepo : TemplateRepo<Kategori>,IKategoriRepo
    {
        private readonly ApplicationDbContext _context;

        public KategoriRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    
        

        public async Task<Kategori> SearchByName(string kategoriNavn)
        {
            if (string.IsNullOrWhiteSpace(kategoriNavn)) return null;

            return await _context.Kategorier
                .FirstOrDefaultAsync(k => k.Navn.ToLower() == KategoriNavn.Trim().ToLower());
        }
        public async Task<Kategori> NyKategori(string kategoriNavn)
        {
            if (string.IsNullOrWhiteSpace(kategoriNavn))
            {
                throw new ArgumentException("Kategori name cannot be null or whitespace.");
            }

            kategoriNavn = kategoriNavn.Trim().ToLower();

            // Create a new Kategori instance with the validated and transformed name
            Kategori kategori = new Kategori { Navn = KategoriNavn };

            await AddAsync(kategori);
            await SaveChangesAsync();

            return kategori;
        }
    }
}
