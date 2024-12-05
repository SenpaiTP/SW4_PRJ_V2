using PRJ4.DTOs;
using PRJ4.Models;


namespace PRJ4.Services
{
    public interface IIndstillingerService
    {
        Task<List<IndstillingerDTO>> GetAllAsync();
        Task<IndstillingerDTO> UpdateIndstillingerAsync( IndstillingerDTO indstillingerDTO);
         Task<IndstillingerDTO> AddIndstillingerAsync( IndstillingerDTO indstillingerDTO);

    }
}