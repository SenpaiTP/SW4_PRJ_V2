using PRJ4.DTOs;
using PRJ4.Models;


namespace PRJ4.Services
{
    public interface IKategoryLimitService
    {
        Task<KategoryLimitGetDTO> GetByIdKategoryLimitAsync(int id);
        Task<KategoryLimitGetDTO> AddKategoryLimitAsync(string brugerId, KategoryLimitReturnDTO limitDTO);
  
    }
}