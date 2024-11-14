using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.Data;
using PRJ4.Models;

namespace PRJ4.Repositories
{
    public interface IVudgifter:ITemplateRepo<Vudgifter>
    {
        Task<IEnumerable<Vudgifter>> GetAllByUserId(int brugerId);
    }
}