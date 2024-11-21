using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.Services;
using PRJ4.Infrastructure;
using PRJ4.DTOs;
using PRJ4.Data;

namespace PRJ4.Services
{
    public class FindtægtService : IFindtægtService
    {
        private readonly IFindtægtRepo _findtægtRepo;
        private readonly TokenProvider _tokenProvider;
        private readonly ApplicationDbContext _context;

        public FindtægtService(IFindtægtRepo findtægtRepo, TokenProvider tokenProvider)
        {
            _findtægtRepo = findtægtRepo;
            _tokenProvider = tokenProvider;
            _context = _context;
        }

        public async Task<IEnumerable<Findtægt>> GetIndtægtAsync(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                throw new ArgumentException("User ID is missing.");
            }

            return await _findtægtRepo.GetByUserIdAsync(int.Parse(userIdClaim));
        }

        public async Task AddFindtægtAsync(Findtægt findtægt, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
            {
                throw new ArgumentException("User ID is missing.");
            }

            findtægt.BrugerId = int.Parse(userIdClaim); // Hvis der er en BrugerId-kolonne i Findtægt
            await _findtægtRepo.AddAsync(findtægt);
        }

         public async Task UpdateFindtægt(int id, int brugerId, FindtægtUpdateDTO dto)
        {
            // Get the existing Fudgifter
            var Findtægt = await _findtægtRepo.GetByIdAsync(id) 
                            ?? throw new KeyNotFoundException("Faste indtægter ikke fundet.");

            // Check if the logged-in user matches the one who created the Fudgifter
            if (Findtægt.BrugerId != brugerId)
                throw new UnauthorizedAccessException("Unauthorized.");

            // Update fields if provided
            if (dto.Indtægt.HasValue) Findtægt.Indtægt = dto.Indtægt.Value;
            if (!string.IsNullOrWhiteSpace(dto.Tekst)) Findtægt.Tekst = dto.Tekst;
            if (dto.Dato.HasValue) Findtægt.Dato = dto.Dato.Value;

            // Handle Kategori (either by ID or by name)
            // if (dto.KategoriId.HasValue)
            // {
            //     // Check if KategoriId is valid
            //     Findtægt.Kategori = await _kategoriRepo.GetByIdAsync(dto.KategoriId.Value)
            //         ?? throw new KeyNotFoundException("Kategori not found.");
            // }
            // else if (!string.IsNullOrWhiteSpace(dto.KategoriNavn))
            // {
            //     // Search for the category by name
            //     var kategori = await _kategoriRepo.SearchByName(dto.KategoriNavn);

            //     // If Kategori not found, create a new one
            //     if (kategori == null)
            //     {
            //         kategori = await _kategoriRepo.NyKategori(dto.KategoriNavn);
            //     }

            //     // Assign the found or newly created category
            //     Fudgifter.Kategori = kategori;
            // }

            // Update the Fudgifter record
            _findtægtRepo.Update(Findtægt);
            await _findtægtRepo.SaveChangesAsync();
        }
    }
}