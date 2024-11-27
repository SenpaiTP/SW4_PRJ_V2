using System.Security.Claims; // Ensure this is included
using System.Threading.Tasks;
using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.DTOs;

namespace PRJ4.Services
{
    public interface IFindtægtService
    {
        Task<Findtægt> CreateFindtægtAsync(string userId, FindtægtCreateDTO findtægtCreateDTO);
        Task<Findtægt> UpdateFindtægtAsync(string userId, int id, FindtægtUpdateDTO findtægtUpdateDTO);
        Task<Findtægt> DeleteFindtægtAsync(string userId, int id);
    }
}