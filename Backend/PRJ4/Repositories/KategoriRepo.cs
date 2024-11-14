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
        public async Task<Kategori> NewKategori(string kategoriName)
        {
            if (string.IsNullOrWhiteSpace(kategoriName))
            {
                throw new ArgumentException("Kategori name cannot be null or whitespace.");
            }

            // Convert the name to lowercase after validation
            kategoriName = kategoriName.Trim().ToLower();

            // Create a new Kategori instance with the validated and transformed name
            Kategori kategori = new Kategori { Name = kategoriName };

            // Add and save changes, allowing the database to generate the ID
            await AddAsync(kategori);
            await SaveChangesAsync();

            // Return the created kategori object
            return kategori;
        }

    }
}