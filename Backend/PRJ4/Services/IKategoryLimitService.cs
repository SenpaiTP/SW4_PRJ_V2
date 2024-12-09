using PRJ4.DTOs;
using PRJ4.Models;


namespace PRJ4.Services
{
    public interface IKategoryLimitService
    {
        Task<List<KategoryLimitResponseDTO>> GetAllKategoryLimits(string userId);
        Task<KategoryLimitResponseDTO> GetByIdKategoryLimits(int kategoryId, string userId);
        Task<KategoryLimitResponseDTO> AddKategoryLimitAsync(KategoryLimitCreateDTO kategoryLimitDTO, string userId);
        Task<KategoryLimitResponseDTO> UpdateKategoryLimitAsync(int kategoryId, string userId, KategoryLimitUpdateDTO kategoryLimitDTO);
        Task DeleteKategoryLimitAsync(int kategoryId, string userId);

  
    }
}