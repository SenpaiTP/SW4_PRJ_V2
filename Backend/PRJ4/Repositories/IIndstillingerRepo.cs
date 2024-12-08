using PRJ4.Models;
using PRJ4.DTOs;

namespace PRJ4.Repositories
{
    public interface IIndstillingerRepo:ITemplateRepo<Indstillinger>
    {
         Task<IEnumerable<IndstillingerDTO>> GetIndstillingerAsync(string userId);
         Task<IEnumerable<UpdateThemeDTO>> GetThemeAsync(string userId);

         Task<Indstillinger> UpdateIndstillingerAsync(string userId, int id, IndstillingerDTO indstillingerDTO);  
         Task<Indstillinger> UpdateThemeAsync(string userId, int id, UpdateThemeDTO updateThemeDTO);
         Task<Indstillinger> AddIndstillingerAsync( string userId, IndstillingerDTO indstillingerDTO);  
         Task<Indstillinger> AddThemeAsync( string userId, UpdateThemeDTO updateThemeDTO);    
    }
}