using System.Security.Claims; // Ensure this is included
using System.Threading.Tasks;
using PRJ4.Models;
using PRJ4.Repositories;

namespace PRJ4.Services
{
    public interface IFindtægtService
    {
        Task AddAsync(Findtægt findtægt);

    }
}