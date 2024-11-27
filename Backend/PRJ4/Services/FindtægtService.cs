
using PRJ4.DTOs;
using PRJ4.Models;
using PRJ4.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using PRJ4.Services;

public class FindtægtService : IFindtægtService
{
    private readonly IFindtægtRepo _findtægtRepo;

    public FindtægtService(IFindtægtRepo findtægtRepo)
    {
        _findtægtRepo = findtægtRepo;
    }

    public async Task<Findtægt> CreateFindtægtAsync(string userId, FindtægtCreateDTO findtægtCreateDTO)
    {
        return await _findtægtRepo.CreateFindtægtAsync(userId, findtægtCreateDTO);
    }

    public async Task<Findtægt> UpdateFindtægtAsync(string userId, int id, FindtægtUpdateDTO findtægtUpdateDTO)
    {
        return await _findtægtRepo.UpdateFindtægtAsync(userId, id, findtægtUpdateDTO);
    }

    public async Task<Findtægt> DeleteFindtægtAsync(string userId, int id)
    {
        return await _findtægtRepo.DeleteFindtægtAsync(userId, id);
    }
}