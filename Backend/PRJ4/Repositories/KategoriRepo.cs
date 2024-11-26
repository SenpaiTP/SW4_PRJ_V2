using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.Data;
using PRJ4.Models;

namespace PRJ4.Repositories
{
    public class KategoriRepo : TemplateRepo<Kategori>,IKategori
    {
        private readonly ApplicationDbContext _context;
        public KategoriRepo(ApplicationDbContext context) : base(context)
        {
           _context = context;
        }

        public async Task<Kategori> SearchByName(string KategoriNavn)
        {
            if (string.IsNullOrWhiteSpace(KategoriNavn)) return null;
            
            return await _context.Kategorier
                .FirstOrDefaultAsync(k => k.Navn.ToLower() == KategoriNavn.Trim().ToLower());
        }

        public async Task<Kategori> NyKategori(string KategoriNavn)
        {
            if (string.IsNullOrWhiteSpace(KategoriNavn))
            {
                throw new ArgumentException("Kategori name cannot be null or whitespace.");
            }
            
            // Convert the name to lowercase after validation
            KategoriNavn = KategoriNavn.Trim().ToLower();

            // Create a new Kategori instance with the validated and transformed name
            Kategori kategori = new Kategori { Navn = KategoriNavn };

            // Add and save changes, allowing the database to generate the ID
            await AddAsync(kategori);
            await SaveChangesAsync();

            // Return the created kategori object
            return kategori;
        }

    }
}