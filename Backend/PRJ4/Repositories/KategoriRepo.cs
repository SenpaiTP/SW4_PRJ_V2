using FuzzyString;
using Microsoft.EntityFrameworkCore;
using PRJ4.Data;
using PRJ4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRJ4.Repositories
{
    public class KategoriRepo : TemplateRepo<Kategori>, IKategori
    {
        private readonly ApplicationDbContext _context;

        public KategoriRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Calculate fuzzy match score using multiple algorithms and return the best match
        public async Task<Kategori> SearchByBestFuzzyMatch(string SuggestedKategoriNavn)
        {
            // Get all categories from the database
            var categories = await _context.Kategorier.ToListAsync();

            // Initialize a list to hold the category and its corresponding match score
            var categoryScores = new List<(Kategori category, double score)>();

            // Iterate through each category to calculate the match score
            foreach (var category in categories)
            {
                var levenshteinScore = category.Navn.RatcliffObershelpSimilarity(SuggestedKategoriNavn);

                var jaroWinklerScore = category.Navn.LevenshteinDistance(SuggestedKategoriNavn);

                var ratcliffObershelpScore = category.Navn.JaroWinklerDistance(SuggestedKategoriNavn);

                // Find the highest score among the three algorithms
                var bestScore = Math.Max(levenshteinScore, Math.Max(jaroWinklerScore, ratcliffObershelpScore));

                // Add the category and its best score to the list
                categoryScores.Add((category, bestScore));
            }

            // Get the category with the highest score
            var bestMatch = categoryScores.OrderBy(x => x.score).FirstOrDefault();

            // Return the category with the highest match score
            return bestMatch.category;
        }
        public async Task<Kategori> SearchByName(string kategoriNavn)
        {
            if (string.IsNullOrWhiteSpace(kategoriNavn)) return null;

            return await _context.Kategorier
                .FirstOrDefaultAsync(k => k.Navn.ToLower() == kategoriNavn.Trim().ToLower());
        }
        public async Task<Kategori> NyKategori(string kategoriNavn)
        {
            if (string.IsNullOrWhiteSpace(kategoriNavn))
            {
                throw new ArgumentException("Kategori name cannot be null or whitespace.");
            }

            kategoriNavn = kategoriNavn.Trim().ToLower();

            var kategori = new Kategori { Navn = kategoriNavn };

            await AddAsync(kategori);
            await SaveChangesAsync();

            return kategori;
        }
    }
}
