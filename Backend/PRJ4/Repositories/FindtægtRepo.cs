
using PRJ4.Data;
using PRJ4.Models;
using PRJ4.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Net.NetworkInformation;
using PRJ4.Repositories;

public class FindtægtRepo : TemplateRepo<Findtægt>, IFindtægtRepo
{
    private readonly ApplicationDbContext _context;

    public FindtægtRepo(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FindtægtResponseDTO>> GetFindtægterByUserIdAsync(string userId)
    {
        return await _context.Findtægter
            .Where(f => f.BrugerId == userId)
            .Include(f => f.Kategori)
            .Select(f => new FindtægtResponseDTO
            {
                // Map properties here
                FindtægtId = f.FindtægtId,
                Indtægt = f.Indtægt,
                Tekst = f.Tekst,
                Dato = f.Dato,
                KategoriNavn = f.Kategori.KategoriNavn,
                KategoriId = f.KategoriId
            })
            .ToListAsync();
    }

    public async Task<FindtægtResponseDTO> GetById(string userId, int id)
    {
        return await _context.Findtægter
        .Where(f => f.BrugerId == userId && f.FindtægtId == id)
        .Include(f => f.Kategori)
        .Select(f => new FindtægtResponseDTO
        {
            // Map properties here
            FindtægtId = f.FindtægtId,
            Indtægt = f.Indtægt,
            Tekst = f.Tekst,
            Dato = f.Dato,
            KategoriNavn = f.Kategori.KategoriNavn,
            KategoriId = f.KategoriId
        })
        .FirstOrDefaultAsync();
    }

    public async Task<Findtægt> CreateFindtægtAsync(string userId, FindtægtCreateDTO findtægtCreateDTO)
    {
        var findtægt = new Findtægt
        {
            BrugerId = userId,
            // Map properties from findtægtCreateDTO
            Indtægt = findtægtCreateDTO.Indtægt,
            Tekst = findtægtCreateDTO.Tekst,
            Kategori = await _context.Kategorier.FirstOrDefaultAsync(k => k.KategoriNavn == findtægtCreateDTO.KategoriNavn),
            KategoriId = findtægtCreateDTO.KategoriId,
            Dato = findtægtCreateDTO.Dato
        };
        _context.Findtægter.Add(findtægt);
        await _context.SaveChangesAsync();
        return findtægt;
    }

    public async Task<Findtægt> UpdateFindtægtAsync(string userId, int id, FindtægtUpdateDTO findtægtUpdateDTO)
    {
        var findtægt = await _context.Findtægter
            .FirstOrDefaultAsync(f => f.FindtægtId == id && f.BrugerId == userId);
        if (findtægt == null)
        {
            throw new KeyNotFoundException("Findtægt not found");
        }
        // Update properties from findtægtUpdateDTO
        findtægt.Indtægt = findtægtUpdateDTO.Indtægt;
        findtægt.Tekst = findtægtUpdateDTO.Tekst;
        findtægt.Dato = findtægtUpdateDTO.Dato;
        findtægt.Kategori = await _context.Kategorier.FirstOrDefaultAsync(k => k.KategoriNavn == findtægtUpdateDTO.KategoriNavn);
        findtægt.KategoriId = findtægtUpdateDTO.KategoriId;
        await _context.SaveChangesAsync();
        return findtægt;
    }

    public async Task<Findtægt> DeleteFindtægtAsync(string userId, int id)
    {
        var findtægt = await _context.Findtægter
            .FirstOrDefaultAsync(f => f.FindtægtId == id && f.BrugerId == userId);
        if (findtægt == null)
        {
            throw new KeyNotFoundException("Findtægt not found");
        }
        _context.Findtægter.Remove(findtægt);
        await _context.SaveChangesAsync();
        return findtægt;
    }
}