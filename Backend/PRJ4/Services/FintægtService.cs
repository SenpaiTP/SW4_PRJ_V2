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

         public async Task<bool> UpdateFindtægtAsync(int id, FindtægtDTO findtægtDto, ClaimsPrincipal user)
        {
            // Get the existing income record
            // Hent den eksisterende indtægt fra repoet
            var existingFindtægt = await _findtægtRepo.GetByIdAsync(id);
            if (existingFindtægt == null)
            {
                return false;  // Returner false, hvis indtægten ikke findes
            }

            var userId = int.Parse(user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);

            // Bekræft at indtægten tilhører den aktuelle bruger
            if (existingFindtægt.BrugerId != userId)
            {
                return false;  // Returner false, hvis indtægten ikke tilhører brugeren
            }

            // Opdater data
            existingFindtægt.Tekst = findtægtDto.Tekst;
            existingFindtægt.Indtægt = findtægtDto.Indtægt;
            existingFindtægt.Dato = findtægtDto.Dato;

            // Gem ændringerne i databasen
            await _findtægtRepo.UpdateAsync(existingFindtægt);
            await _findtægtRepo.SaveChangesAsync();

            return true;  // Returner true, når opdateringen er gennemført
        }
    }
}