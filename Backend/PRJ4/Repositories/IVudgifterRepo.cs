using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.Data;
using PRJ4.Models;

namespace PRJ4.Repositories
{
    public interface IVudgifterRepo:ITemplateRepo<Vudgifter>
    {
        Task<IEnumerable<Vudgifter>> GetAllByUserId(string brugerId);
        Task<IEnumerable<Vudgifter>> GetAllByCategory(string brugerId, int CategoryId);
        Task<IEnumerable<Vudgifter>> GetAllByDate(string brugerId, DateTime from, DateTime end);
        Task<IEnumerable<Vudgifter>> GetAllByCategoryADate(string brugerId, int CategoryId, DateTime from, DateTime end);
    }
}