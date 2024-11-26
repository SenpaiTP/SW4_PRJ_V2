
using PRJ4.Data;
using PRJ4.Models;
using PRJ4.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FindtægtRepo : IFindtægtRepo
{
    private readonly ApplicationDbContext _context;

    public FindtægtRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FindtægtDTO>> GetFindtægterByUserIdAsync(string userId)
    {
        return await _context.Findtægter
            .Where(f => f.UserId == userId)
            .Select(f => new FindtægtDTO
            {
                // Map properties here
            })
            .ToListAsync();
    }

    public async Task<Findtægt> CreateFindtægtAsync(string userId, FindtægtCreateDTO findtægtCreateDTO)
    {
        var findtægt = new Findtægt
        {
            UserId = userId,
            // Map properties from findtægtCreateDTO
        };
        _context.Findtægter.Add(findtægt);
        await _context.SaveChangesAsync();
        return findtægt;
    }

    public async Task<bool> UpdateFindtægtAsync(string userId, int id, FindtægtUpdateDTO findtægtUpdateDTO)
    {
        var findtægt = await _context.Findtægter
            .FirstOrDefaultAsync(f => f.FindtægtId == id && f.UserId == userId);
        if (findtægt == null)
        {
            return false;
        }
        // Update properties from findtægtUpdateDTO
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteFindtægtAsync(string userId, int id)
    {
        var findtægt = await _context.Findtægter
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
        if (findtægt == null)
        {
            return false;
        }
        _context.Findtægter.Remove(findtægt);
        await _context.SaveChangesAsync();
        return true;
    }
}