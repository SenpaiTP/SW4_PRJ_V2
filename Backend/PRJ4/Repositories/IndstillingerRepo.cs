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
using PRJ4.Models;

namespace PRJ4.Repositories;


public class IndstillingerRepo:TemplateRepo<Indstillinger>,IIndstillingerRepo
{
    private readonly ApplicationDbContext _context;
    public IndstillingerRepo(ApplicationDbContext context) : base(context) 
    {
        _context = context;
    }

    public async Task<IEnumerable<IndstillingerDTO>> GetIndstillingerAsync(string userId)
    {
        return await _context.Indstillingers
            .Where(f => f.BrugerId == userId )
            .Select(f => new IndstillingerDTO
            {
                SetPieChart = f.SetPieChart,
                SetSøjlediagram = f.SetSøjlediagram,
                SetBudget = f.SetBudget,
                SetIndtægter = f.SetIndtægter,
                SetUdgifter = f.SetUdgifter
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<UpdateThemeDTO>> GetThemeAsync(string userId) 
    {   
        return await _context.Indstillingers
            .Where(f => f.BrugerId == userId )
            .Select(f => new UpdateThemeDTO
            {
                SetTheme = f.SetTheme
            })
            .ToListAsync();
    }

    public async Task<Indstillinger> UpdateThemeAsync(string userId, int id, UpdateThemeDTO updateThemeDTO)
    {
        var newTheme = await _context.Indstillingers
            .FirstOrDefaultAsync(f => f.IndstillingerId == id && f.BrugerId == userId);

        newTheme.SetTheme = updateThemeDTO.SetTheme;
        
        await _context.SaveChangesAsync();

        /*var updatedThemeDTO = new UpdateThemeDTO 
        {
            SetTheme = newTheme.SetTheme
        };*/
        return newTheme;
    }

    public async Task<Indstillinger> UpdateIndstillingerAsync(string userId, int id, IndstillingerDTO indstillingerDTO)
    {
        var indstillinger = await _context.Indstillingers
            .FirstOrDefaultAsync();

        //indstillinger.SetTheme = indstillingerDTO.SetTheme;
        indstillinger.SetPieChart = indstillingerDTO.SetPieChart;
        indstillinger.SetSøjlediagram = indstillingerDTO.SetSøjlediagram;
        indstillinger.SetBudget = indstillingerDTO.SetBudget;
        indstillinger.SetIndtægter = indstillingerDTO.SetIndtægter;
        indstillinger.SetUdgifter = indstillingerDTO.SetUdgifter;

        await _context.SaveChangesAsync();
        return indstillinger;
    }

   public async Task<Indstillinger> AddIndstillingerAsync( string userId, IndstillingerDTO indstillingerDTO){
        
        var indstillinger = new Indstillinger
        {
            BrugerId = userId,
            SetPieChart = indstillingerDTO.SetPieChart,
            SetSøjlediagram = indstillingerDTO.SetSøjlediagram,
            SetBudget = indstillingerDTO.SetBudget,
            SetIndtægter = indstillingerDTO.SetIndtægter,
            SetUdgifter = indstillingerDTO.SetUdgifter
        };

        _context.Indstillingers.Add(indstillinger);
        await _context.SaveChangesAsync();
        return indstillinger;

    }

    public async Task<Indstillinger> AddThemeAsync(string userId, UpdateThemeDTO updateThemeDTO)
    {
        var theme = new Indstillinger
        {
            BrugerId = userId,
            SetTheme = updateThemeDTO.SetTheme
        }
        ;
         _context.Indstillingers.Add(theme);
        await _context.SaveChangesAsync();
        return theme;
    }
}