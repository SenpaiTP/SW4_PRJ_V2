using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using PRJ4.Data;



namespace PRJ4.Repositories;


public class TemplateRepo<T> : ITemplateRepo<T> where T:class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public TemplateRepo(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public async Task<T> Delete(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            return null;
        }
        _dbSet.Remove(entity);
        return entity;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}