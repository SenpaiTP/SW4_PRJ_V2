using PRJ4.DTOs;
using PRJ4.Models;


namespace PRJ4.Services
{
    public interface IKategoryLimitService
    {
        Task<KategoryLimitDTO> GetByIdKategoryLimitAsync(int id);
        Task<KategoryLimitDTO> AddKategoryLimitAsync(string brugerId, KategoryLimitDTO limitDTO);
  
    }
}