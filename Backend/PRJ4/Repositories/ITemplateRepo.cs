using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using PRJ4.Data;
//using PRJ4.Models;

namespace PRJ4.Repositories
{
    public interface ITemplateRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
        Task SaveChangesAsync();
    }   
}