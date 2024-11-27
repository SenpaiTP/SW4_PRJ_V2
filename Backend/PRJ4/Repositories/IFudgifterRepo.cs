using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.Data;
using PRJ4.Models;

namespace PRJ4.Repositories
{
    public interface IFudgifter:ITemplateRepo<Fudgifter>
    {
        Task<IEnumerable<Fudgifter>> GetAllByUserId(string brugerId);
        Task<IEnumerable<Fudgifter>> GetAllByCategory(string brugerId, int kategoryId);
        Task<IEnumerable<Fudgifter>> GetAllByDate(string brugerId, DateTime from, DateTime end);
        Task<IEnumerable<Fudgifter>> GetAllByCategoryADate(string brugerId, int kategoryId,DateTime from, DateTime end);
    }
}