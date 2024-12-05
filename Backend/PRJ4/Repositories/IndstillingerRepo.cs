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

    public async Task<List<Indstillinger>> GetAllAsync()
    {
        return await _context.Indstillingers.ToListAsync();

    }

    public async Task<Indstillinger> UpdateIndstillingerAsync(IndstillingerDTO indstillingerDTO)
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

   public async Task<Indstillinger> AddIndstillingerAsync( IndstillingerDTO indstillingerDTO){
        
        var indstillinger = new Indstillinger
        {
            //SetTheme = indstillingerDTO.SetTheme,
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
}