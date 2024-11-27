using PRJ4.DTOs;
using PRJ4.Models;
using PRJ4.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using PRJ4.Services;

public class VindtægtService : IVindtægtService
{
    private readonly IVindtægtRepo _vindtægtRepo;

    public VindtægtService(IVindtægtRepo VindtægtRepo)
    {
        _vindtægtRepo = VindtægtRepo;
    }

    public async Task<Vindtægt> CreateVindtægtAsync(string userId, VindtægtCreateDTO VindtægtCreateDTO)
    {
        return await _vindtægtRepo.CreateVindtægtAsync(userId, VindtægtCreateDTO);
    }

    public async Task<Vindtægt> UpdateVindtægtAsync(string userId, int id, VindtægtUpdateDTO VindtægtUpdateDTO)
    {
        return await _vindtægtRepo.UpdateVindtægtAsync(userId, id, VindtægtUpdateDTO);
    }

    public async Task<Vindtægt> DeleteVindtægtAsync(string userId, int id)
    {
        return await _vindtægtRepo.DeleteVindtægtAsync(userId, id);
    }
}