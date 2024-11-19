using System.Security.Claims; // Ensure this is included
using System.Threading.Tasks;
using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.DTOs;

namespace PRJ4.Services
{
    public interface IFindtægtService
    {
        Task AddFindtægtAsync(Findtægt findtægt, ClaimsPrincipal user);
        Task<IEnumerable<Findtægt>> GetIndtægtAsync(int userId);

    }
}