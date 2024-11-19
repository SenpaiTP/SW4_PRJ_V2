using System.Security.Claims;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Findtægt>> GetIndtægtAsync(int userId)
        {
            // Fetch the income records for the given user ID
            return Findtægt = await _context.Findtægter.Where(f => f.BrugerId == userId).ToListAsync();
        }

        public async Task AddFindtægtAsync(Findtægt findtægt, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                throw new ArgumentException("User ID is missing.");
            }

            findtægt.BrugerId = int.Parse(userIdClaim); // Hvis der er en BrugerId-kolonne i Findtægt
            await _findtægtRepo.AddAsync(findtægt);
        }
    }
}