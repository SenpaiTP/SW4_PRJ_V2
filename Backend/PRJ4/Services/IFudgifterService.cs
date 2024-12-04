using PRJ4.DTOs;
using PRJ4.Models;  


namespace PRJ4.Services
{
    public interface IFudgifterService
    {
        // Define methods related to "Fudgifter"
        Task<IEnumerable<FudgifterResponseDTO>> GetAllByUser(string brugerId);
        Task<FudgifterResponseDTO> AddFudgifter(string brugerId, nyFudgifterDTO dto);
        Task UpdateFudgifter(string brugerId, int id, FudgifterUpdateDTO dto);
        Task DeleteFudgifter(string brugerId, int id);
    }
}
