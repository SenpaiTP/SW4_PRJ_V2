using System.Security.Claims; // Ensure this is included
using System.Threading.Tasks;
using PRJ4.Models;
namespace PRJ4.Services;
public interface IRevocationService
{
    // Task<bool> RevokeRefreshTokenAsync(string token, ClaimsPrincipal user);
    Task RevokeRefreshTokenAsync(string token);
    Task<bool> IsTokenRevokedAsync(string token);
}