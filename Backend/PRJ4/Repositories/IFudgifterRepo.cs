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
        Task<IEnumerable<Fudgifter>> GetAllByUserId(int brugerId);
    }
}