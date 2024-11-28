using PRJ4.DTOs;
using PRJ4.Models;
using PRJ4.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using PRJ4.Services;

public class VindtægtService : IVindtægtService
{
    private readonly IVindtægtRepo _vindtægtRepo;
    private readonly IKategoriRepo _kategoriRepo;

    public VindtægtService(IVindtægtRepo VindtægtRepo, IKategoriRepo KategoriRepo)
    {
        _vindtægtRepo = VindtægtRepo;
        _kategoriRepo = KategoriRepo;
    }

    public async Task<Vindtægt> CreateVindtægtAsync(string userId, VindtægtCreateDTO VindtægtCreateDTO)
    {
        Kategori kategori;

        if(VindtægtCreateDTO.KategoriId >=0)
        {
            kategori = await _kategoriRepo.SearchByName(VindtægtCreateDTO.KategoriNavn);

            if(kategori == null)
            {
                kategori = await _kategoriRepo.NyKategori(VindtægtCreateDTO.KategoriNavn);
            }
        }

        else
        {
            kategori = await  _kategoriRepo.GetByIdAsync(VindtægtCreateDTO.KategoriId.Value)
                        ?? throw new KeyNotFoundException("Kategori ikke fundet.");
        }

        return await _vindtægtRepo.CreateVindtægtAsync(userId, VindtægtCreateDTO);
    }

    public async Task<Vindtægt> UpdateVindtægtAsync(string userId, int id, VindtægtUpdateDTO VindtægtUpdateDTO)
    {
        var vindtægt = await _vindtægtRepo.GetByIdAsync(id);

        if(VindtægtUpdateDTO.KategoriId.HasValue)
        {
            vindtægt.Kategori = await _kategoriRepo.GetByIdAsync(VindtægtUpdateDTO.KategoriId.Value);
            if(vindtægt.Kategori == null)
            {
                throw new Exception("Kategori ikke fundet");
            }
            else if(vindtægt.Kategori.KategoriNavn != VindtægtUpdateDTO.KategoriNavn)
            {
                throw new Exception("KategoriNavn matcher ikke KategoriId");
            }

            VindtægtUpdateDTO.KategoriNavn = vindtægt.Kategori.KategoriNavn;
        }
        
        return await _vindtægtRepo.UpdateVindtægtAsync(userId, id, VindtægtUpdateDTO);
    }

    public async Task<Vindtægt> DeleteVindtægtAsync(string userId, int id)
    {
        return await _vindtægtRepo.DeleteVindtægtAsync(userId, id);
    }
}