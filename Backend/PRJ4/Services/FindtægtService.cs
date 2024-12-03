
using PRJ4.DTOs;
using PRJ4.Models;
using PRJ4.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using PRJ4.Services;

public class FindtægtService : IFindtægtService
{
    private readonly IFindtægtRepo _findtægtRepo;
    private readonly IKategoriRepo _kategoriRepo;

    public FindtægtService(IFindtægtRepo findtægtRepo , IKategoriRepo kategoriRepo)
    {
        _findtægtRepo = findtægtRepo;
        _kategoriRepo = kategoriRepo;
    }

    public async Task<Findtægt> CreateFindtægtAsync(string userId, FindtægtCreateDTO findtægtCreateDto)
    {
        Kategori kategori;

            if (findtægtCreateDto.KategoriId >= 0)
            {
               //_logger.LogInformation("Searching for category by name: {KategoriNavn}", findtægtCreateDto.KategoriNavn);
                kategori = await _kategoriRepo.SearchByName(findtægtCreateDto.KategoriNavn);

                if (kategori == null)
                {
                    //_logger.LogInformation("Category not found. Creating new category: {KategoriNavn}", findtægtCreateDto.KategoriNavn);
                    kategori = await _kategoriRepo.NyKategori(findtægtCreateDto.KategoriNavn);
                }
            }
            else
            {
               // _logger.LogInformation("Fetching category by ID: {KategoriId}", findtægtCreateDto.KategoriId);
                kategori = await _kategoriRepo.GetByIdAsync(findtægtCreateDto.KategoriId.Value)
                           ?? throw new KeyNotFoundException("Kategori not found.");
            }
        
        return await _findtægtRepo.CreateFindtægtAsync(userId, findtægtCreateDto);
    }

    public async Task<Findtægt> UpdateFindtægtAsync(string userId, int id, FindtægtUpdateDTO findtægtUpdateDto)
    {
        var findtægt = await _findtægtRepo.GetByIdAsync(id);


        if(findtægtUpdateDto.KategoriId.HasValue)
        {
            findtægt.Kategori = await _kategoriRepo.GetByIdAsync(findtægtUpdateDto.KategoriId.Value);
            if(findtægt.Kategori == null)
            {
                throw new Exception("Kategori not found");
            }
            else if(findtægt.Kategori.KategoriNavn != findtægtUpdateDto.KategoriNavn)
            {
                throw new Exception("KategoriNavn does not match KategoriId");
            }

            findtægtUpdateDto.KategoriNavn = findtægt.Kategori.KategoriNavn;
        }
        return await _findtægtRepo.UpdateFindtægtAsync(userId, id, findtægtUpdateDto);
    }

    public async Task<Findtægt> DeleteFindtægtAsync(string userId, int id)
    {
        return await _findtægtRepo.DeleteFindtægtAsync(userId, id);
    }
}