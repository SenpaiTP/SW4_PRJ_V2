using PRJ4.DTOs;
using PRJ4.Models;  


namespace PRJ4.Services
{
    public interface IVudgifterService
    {
        // Define methods related to "Fudgifter"
        Task<IEnumerable<VudgifterResponseDTO>> GetAllByUser(string brugerId);
        Task<VudgifterResponseDTO> AddVudgifter(string brugerId, nyVudgifterDTO dto);
        Task UpdateVudgifter(string brugerId, int id, VudgifterUpdateDTO dto);
        Task DeleteVudgifter(string brugerId, int id);
    }
}
