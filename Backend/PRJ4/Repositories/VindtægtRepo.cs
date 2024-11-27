
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

public class VindtægtRepo : IVindtægtRepo
{
    private readonly ApplicationDbContext _context;

    public VindtægtRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VindtægtResponseDTO>> GetVindtægterByUserIdAsync(string userId)
    {
        return await _context.Vindtægter
            .Where(f => f.BrugerId == userId)
            .Select(f => new VindtægtResponseDTO
            {
                // Map properties here
                VindtægtId = f.VindtægtId,
                Indtægt = f.Indtægt,
                Tekst = f.Tekst,
                Dato = f.Dato
            })
            .ToListAsync();
    }

    public async Task<Vindtægt> CreateVindtægtAsync(string userId, VindtægtCreateDTO VindtægtCreateDTO)
    {
        var Vindtægt = new Vindtægt
        {
            BrugerId = userId,
            // Map properties from VindtægtCreateDTO
            Indtægt = VindtægtCreateDTO.Indtægt,
            Tekst = VindtægtCreateDTO.Tekst,
            Dato = VindtægtCreateDTO.Dato
        };
        _context.Vindtægter.Add(Vindtægt);
        await _context.SaveChangesAsync();
        return Vindtægt;
    }

    public async Task<Vindtægt> UpdateVindtægtAsync(string userId, int id, VindtægtUpdateDTO VindtægtUpdateDTO)
    {
        var Vindtægt = await _context.Vindtægter
            .FirstOrDefaultAsync(f => f.VindtægtId == id && f.BrugerId == userId);
        if (Vindtægt == null)
        {
            throw new KeyNotFoundException("Vindtægt not found");
        }
        // Update properties from VindtægtUpdateDTO
        Vindtægt.Indtægt = VindtægtUpdateDTO.Indtægt;
        Vindtægt.Tekst = VindtægtUpdateDTO.Tekst;
        Vindtægt.Dato = VindtægtUpdateDTO.Dato;
        await _context.SaveChangesAsync();
        return Vindtægt;
    }

    public async Task<Vindtægt> DeleteVindtægtAsync(string userId, int id)
    {
        var Vindtægt = await _context.Vindtægter
            .FirstOrDefaultAsync(f => f.VindtægtId == id && f.BrugerId == userId);
        if (Vindtægt == null)
        {
            throw new KeyNotFoundException("Vindtægt not found");
        }
        _context.Vindtægter.Remove(Vindtægt);
        await _context.SaveChangesAsync();
        return Vindtægt;
    }
}