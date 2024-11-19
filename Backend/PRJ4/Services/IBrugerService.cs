using System.Security.Claims; // Ensure this is included
using System.Threading.Tasks;

namespace PRJ4.Services
{
    public interface IBrugerService
    {
        Task<string> GetBrugerNavnAsync(ClaimsPrincipal user);
        Task<string> GetBrugerEmailAsync(ClaimsPrincipal user);
    }
}
