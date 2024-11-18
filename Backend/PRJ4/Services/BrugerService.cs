using System.Security.Claims;
using System.Threading.Tasks;
using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.Services;
using PRJ4.Infrastructure;

namespace PRJ4.Services
{
    public class BrugerService : IBrugerService
    {
        private readonly IBrugerRepo _brugerRepo;
        private readonly TokenProvider _tokenProvider;

        public BrugerService(IBrugerRepo brugerRepo, TokenProvider tokenProvider)
        {
            _brugerRepo = brugerRepo;
            _tokenProvider = tokenProvider;
        }

        public async Task<string> GetBrugerNavnAsync(ClaimsPrincipal user)
        {
            var brugerIdClaim=user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (brugerIdClaim==null)
            {
                return null;
            }

            var bruger=await _brugerRepo.GetByIdAsync(int.Parse(brugerIdClaim));
            
            if (bruger == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return bruger.Navn;
        }
        
        
        public async Task<string> GetBrugerEmailAsync(ClaimsPrincipal user)
        {
            var brugerIdClaim=user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (brugerIdClaim==null)
            {
                return null;
            }

            var bruger=await _brugerRepo.GetByIdAsync(int.Parse(brugerIdClaim));
            
            if (bruger == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return bruger.Email;
        }
    }
}