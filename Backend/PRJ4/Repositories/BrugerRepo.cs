using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.Data;
using PRJ4.Models;

namespace PRJ4.Repositories
{
    public class BrugerRepo : TemplateRepo<Bruger>,IBrugerRepo
    {
        private readonly ApplicationDbContext _context;
        public BrugerRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Bruger> AuthenticateAsync(string email, string password)
        {
            var bruger = await _context.Brugers.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            if (bruger == null)
            {
                return null;
            }
            return bruger;
        }
    
    }
}