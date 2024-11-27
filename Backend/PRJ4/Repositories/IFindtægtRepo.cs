
using PRJ4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRJ4.DTOs;

public interface IFindtægtRepo
{
    // ...existing code...
    Task<IEnumerable<FindtægtDTO>> GetFindtægterByUserIdAsync(int userId);
    Task<Findtægt> CreateFindtægtAsync(int userId, FindtægtCreateDTO findtægtCreateDTO);
    Task<bool> UpdateFindtægtAsync(int userId, int id, FindtægtUpdateDTO findtægtUpdateDTO);
    Task<bool> DeleteFindtægtAsync(int userId, int id);
    // ...existing code...
}