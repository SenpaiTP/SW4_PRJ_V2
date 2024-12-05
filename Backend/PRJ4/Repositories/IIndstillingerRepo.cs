using PRJ4.Models;
using PRJ4.DTOs;

namespace PRJ4.Repositories
{
    public interface IIndstillingerRepo:ITemplateRepo<Indstillinger>
    {
         Task<List<Indstillinger>> GetAllAsync();
         Task<Indstillinger> UpdateIndstillingerAsync(IndstillingerDTO indstillingerDTO);  
         Task<Indstillinger> AddIndstillingerAsync( IndstillingerDTO indstillingerDTO);    
    }
}