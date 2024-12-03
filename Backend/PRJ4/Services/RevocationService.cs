using System.Collections.Concurrent;
using System.Security.Claims; // Ensure this is included
using System.Threading.Tasks;
using PRJ4.Models;

namespace PRJ4.Services
{
    public class RevocationService : IRevocationService
    {
        private readonly ConcurrentDictionary<string, DateTime> _revokedTokens=new();
        public Task RevokeRefreshTokenAsync(string token)
        {
            _revokedTokens.TryAdd(token, DateTime.UtcNow);
            return Task.CompletedTask;
        }
        public Task<bool> IsTokenRevokedAsync(string token)
        {
            return Task.FromResult(_revokedTokens.ContainsKey(token));
        }
    }
}