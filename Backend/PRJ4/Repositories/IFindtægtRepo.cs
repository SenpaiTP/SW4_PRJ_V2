
using PRJ4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRJ4.DTOs;
using PRJ4.Repositories;

public interface IFindtægtRepo: ITemplateRepo<Findtægt>
{
    // ...existing code...
    Task<IEnumerable<FindtægtResponseDTO>> GetFindtægterByUserIdAsync(string userId);
    Task<FindtægtResponseDTO> GetById(string userId, int id);
    Task<Findtægt> CreateFindtægtAsync(string userId, FindtægtCreateDTO findtægtCreateDTO);
    Task<Findtægt> UpdateFindtægtAsync(string userId, int id, FindtægtUpdateDTO findtægtUpdateDTO);
    Task<Findtægt> DeleteFindtægtAsync(string userId, int id);
    // ...existing code...
}