using PRJ4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRJ4.DTOs;
using PRJ4.Models;
using PRJ4.Repositories;

public interface IVindtægtRepo:ITemplateRepo<Vindtægt>
{
    Task<IEnumerable<VindtægtResponseDTO>> GetVindtægterByUserIdAsync(string userId);
    Task<Vindtægt> CreateVindtægtAsync(string userId, VindtægtCreateDTO VindtægtCreateDTO);
    Task<Vindtægt> UpdateVindtægtAsync(string userId, int id, VindtægtUpdateDTO VindtægtUpdateDTO);
    Task<Vindtægt> DeleteVindtægtAsync(string userId, int id);
}