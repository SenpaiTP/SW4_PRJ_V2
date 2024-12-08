using PRJ4.DTOs;
using PRJ4.Models;


namespace PRJ4.Services
{
    public interface IIndstillingerService
    {
        //Task<List<IndstillingerDTO>> GetAllAsync();
        Task<IndstillingerDTO> UpdateIndstillingerAsync(string userId, int id, IndstillingerDTO indstillingerDTO);
        Task<IndstillingerDTO> AddIndstillingerAsync(string userId, IndstillingerDTO indstillingerDTO);
         Task<Indstillinger> AddThemeAsync(string userId, UpdateThemeDTO updateThemeDTO);
        Task<UpdateThemeDTO> UpdateThemeAsync(string userId, int id, UpdateThemeDTO updateThemeDTO);
        Task<IndstillingerDTO> GetIndstillingerAsync(string userId);
    }
}