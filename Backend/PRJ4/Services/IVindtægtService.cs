using System.Security.Claims; 
using System.Threading.Tasks;
using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.DTOs;

namespace PRJ4.Services
{
    public interface IVindtægtService
    {
        Task<Vindtægt> CreateVindtægtAsync(string userId, VindtægtCreateDTO VindtægtCreateDto);
        Task<Vindtægt> UpdateVindtægtAsync(string userId, int id, VindtægtUpdateDTO VindtægtUpdateDto);
        Task<Vindtægt> DeleteVindtægtAsync(string userId, int id);
    }
}