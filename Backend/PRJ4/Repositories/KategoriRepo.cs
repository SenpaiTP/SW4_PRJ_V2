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
        public async Task<Kategori> SearchByBestFuzzyMatch(string suggestedKategoriNavn)
        {
            // Get all categories from the database
            var categories = await _context.Kategorier.ToListAsync();

            // Initialize a list to hold the category and its corresponding match score
            var categoryScores = new List<(Kategori category, double score)>();

            // Iterate through each category to calculate the match score
            foreach (var category in categories)
            {
                // Calculate scores from different algorithms
                double levenshteinDistance = category.Navn.LevenshteinDistance(suggestedKategoriNavn);
                double normalizedLevenshtein = 1 - (levenshteinDistance / Math.Max(category.Navn.Length, suggestedKategoriNavn.Length));
                
                double ratcliffObershelpScore = category.Navn.RatcliffObershelpSimilarity(suggestedKategoriNavn); // Already between 0 and 1
                
                double jaroWinklerScore = category.Navn.JaroWinklerDistance(suggestedKategoriNavn); // Already between 0 and 1

                // Calculate the combined score (average of all three)
                double combinedScore = (normalizedLevenshtein + ratcliffObershelpScore + jaroWinklerScore) / 3;

                // Add the category and its score to the list
                categoryScores.Add((category, combinedScore));

                // Log for debugging
                Console.WriteLine($"{category.Navn} - Combined Score: {combinedScore:F2}");
            }

            // Find the category with the highest combined score
            var bestMatch = categoryScores.OrderByDescending(x => x.score).FirstOrDefault();

            // Define a similarity threshold (e.g., 0.7)
            double similarityThreshold = 0.9;
            if (bestMatch.score < similarityThreshold)
            {
                return null; // Reject if below threshold
            }

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
