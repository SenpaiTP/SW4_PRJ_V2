using PRJ4.DTOs;
using PRJ4.Models;  


namespace PRJ4.Services
{
    public interface IFudgifterService
    {
        // Define methods related to "Fudgifter"
        Task<IEnumerable<FudgifterResponseDTO>> GetAllByUser(int brugerId);
        Task<FudgifterResponseDTO> AddFudgifter(int brugerId, nyFudgifterDTO dto);
        Task UpdateFudgifter(int brugerId, int id, FudgifterUpdateDTO dto);
        Task DeleteFudgifter(int brugerId, int id);
    }
}
