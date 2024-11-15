using System.Security.Claims;
using System.Threading.Tasks;
using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.Services;
using PRJ4.Infrastructure;

namespace PRJ4.Services
{
    public class FintægtService : IFindtægtService
    {
        private readonly IFindtægtRepo _findtægtRepo;
        private readonly TokenProvider _tokenProvider;

        public FintægtService(IFindtægtRepo findtægtRepo, TokenProvider tokenProvider)
        {
            _findtægtRepo = findtægtRepo;
            _tokenProvider = tokenProvider;
        }

        public async Task AddAsync(Findtægt findtægt)
        {
            await _findtægtRepo.AddAsync(findtægt);
        }

    }
}