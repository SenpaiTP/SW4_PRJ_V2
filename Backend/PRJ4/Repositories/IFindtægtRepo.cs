
using PRJ4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRJ4.DTOs;

public interface IFindtægtRepo
{
    // ...existing code...
    Task<IEnumerable<FindtægtResponseDTO>> GetFindtægterByUserIdAsync(string userId);
    Task<Findtægt> CreateFindtægtAsync(string userId, FindtægtCreateDTO findtægtCreateDTO);
    Task<bool> UpdateFindtægtAsync(string userId, int id, FindtægtUpdateDTO findtægtUpdateDTO);
    Task<bool> DeleteFindtægtAsync(string userId, int id);
    // ...existing code...
}