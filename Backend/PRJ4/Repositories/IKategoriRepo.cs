using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.Data;
using PRJ4.Models;

namespace PRJ4.Repositories
{
    public interface IKategori:ITemplateRepo<Kategori>
    {
        Task<Kategori> NyKategori(string KategoriNavn);
        Task<Kategori> SearchByName(string kategoriNavn);
    }
}