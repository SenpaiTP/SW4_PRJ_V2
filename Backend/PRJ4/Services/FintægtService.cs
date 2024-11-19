using System.Security.Claims;
using System.Threading.Tasks;
using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.Services;
using PRJ4.Infrastructure;

namespace PRJ4.Services
{
    public class FindtægtService : IFindtægtService
    {
        private readonly IFindtægtRepo _findtægtRepo;
        private readonly TokenProvider _tokenProvider;

        public FindtægtService(IFindtægtRepo findtægtRepo, TokenProvider tokenProvider)
        {
            _findtægtRepo = findtægtRepo;
            _tokenProvider = tokenProvider;
        }

        public async Task<decimal?> GetIndtægtAsync(Findtægt findtægt, ClaimsPrincipal user)
        {
            // Implement method
            var brugerIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(brugerIdClaim == null)
            {
                return null;
            }
            var indtægt=await _findtægtRepo.GetByIdAsync(int.Parse(brugerIdClaim));

            if(indtægt == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return indtægt.Indtægt;
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