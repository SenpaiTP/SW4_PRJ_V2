using PRJ4.DTOs;
using PRJ4.Models;  


namespace PRJ4.Services
{
    public interface IVudgifterService
    {
        // Define methods related to "Fudgifter"
        Task<IEnumerable<VudgifterResponseDTO>> GetAllByUser(int brugerId);
        Task<VudgifterResponseDTO> AddVudgifter(int brugerId, nyVudgifterDTO dto);
        Task UpdateVudgifter(int brugerId, int id, VudgifterUpdateDTO dto);
        Task DeleteVudgifter(int brugerId, int id);
    }
}
