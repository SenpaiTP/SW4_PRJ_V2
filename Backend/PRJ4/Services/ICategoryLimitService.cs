using PRJ4.DTOs;
using PRJ4.Models;


namespace PRJ4.Services
{
    public interface ICategoryLimitService
    {
        Task<List<CategoryLimitResponseDTO>> GetAllCategoryLimits(string userId);
        Task<CategoryLimitResponseDTO> GetByIdCategoryLimits(int CategoryId, string userId);
        Task<CategoryLimitResponseDTO> AddCategoryLimitAsync(CategoryLimitCreateDTO CategoryLimitDTO, string userId);
        Task<CategoryLimitResponseDTO> UpdateCategoryLimitAsync(int CategoryId, string userId, CategoryLimitUpdateDTO CategoryLimitDTO);
        Task DeleteCategoryLimitAsync(int CategoryId, string userId);

  
    }
}