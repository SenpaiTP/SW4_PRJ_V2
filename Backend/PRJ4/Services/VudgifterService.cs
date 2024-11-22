using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.DTOs;
using AutoMapper;

namespace PRJ4.Services
{
    public class VudgifterService : IVudgifterService
    {
        private readonly IVudgifter _VudgifterRepo;
        private readonly IBrugerRepo _brugerRepo;
        private readonly IKategori _kategoriRepo;
        private readonly ILogger<VudgifterService> _logger;
        private readonly IMapper _mapper; 

        public VudgifterService(
            IVudgifter VudgifterRepo,
            IKategori kategoriRepo,
            IBrugerRepo brugerRepo,
            ILogger<VudgifterService> logger,
            IMapper mapper)
        {
            _VudgifterRepo = VudgifterRepo;
            _kategoriRepo = kategoriRepo;
            _brugerRepo = brugerRepo;
            _logger = logger;
            _mapper = mapper; 
        }

        // Get all expenses for a user
        public async Task<IEnumerable<VudgifterResponseDTO>> GetAllByUser(int brugerId)
        {
            _logger.LogInformation("Fetching all expenses for user with ID: {BrugerId}", brugerId);

            var Vudgifter = await _VudgifterRepo.GetAllByUserId(brugerId);

            _logger.LogInformation("Found {Count} expenses for user with ID: {BrugerId}", Vudgifter.Count(), brugerId);

            // Use AutoMapper to map Vudgifter to VudgifterResponseDTO
            return _mapper.Map<IEnumerable<VudgifterResponseDTO>>(Vudgifter);
        }

        // Add a new expense for a user
        public async Task<VudgifterResponseDTO> AddVudgifter(int brugerId, nyVudgifterDTO dto)
        {
            _logger.LogInformation("Adding new expense for user with ID: {BrugerId}", brugerId);

            var bruger = await _brugerRepo.GetByIdAsync(brugerId);
            if (bruger == null)
            {
                _logger.LogWarning("User with ID {BrugerId} not found", brugerId);
                throw new KeyNotFoundException("Bruger not found.");
            }

            Kategori kategori;

            if (dto.KategoriId <= 0)
            {
                _logger.LogInformation("Searching for category by name: {KategoriNavn}", dto.KategoriNavn);
                kategori = await _kategoriRepo.SearchByName(dto.KategoriNavn);

                if (kategori == null)
                {
                    _logger.LogInformation("Category not found. Creating new category: {KategoriNavn}", dto.KategoriNavn);
                    kategori = await _kategoriRepo.NyKategori(dto.KategoriNavn);
                }
            }
            else
            {
                _logger.LogInformation("Fetching category by ID: {KategoriId}", dto.KategoriId);
                kategori = await _kategoriRepo.GetByIdAsync(dto.KategoriId)
                           ?? throw new KeyNotFoundException("Kategori not found.");
            }
            dto.KategoriId = kategori.KategoriId;
            var nyVudgifter = _mapper.Map<Vudgifter>(dto);
            // Set additional properties
            nyVudgifter.BrugerId = brugerId;
            nyVudgifter.Kategori = kategori;
            nyVudgifter.Bruger = bruger;

            await _VudgifterRepo.AddAsync(nyVudgifter);
            await _VudgifterRepo.SaveChangesAsync();

            _logger.LogInformation("Successfully added expense with ID: {VudgiftId} for user with ID: {BrugerId}", nyVudgifter.VudgiftId, brugerId);

            // Use AutoMapper to return the DTO
            return _mapper.Map<VudgifterResponseDTO>(nyVudgifter);
        }

        // Update an existing expense for a user
        public async Task UpdateVudgifter(int id, int brugerId, VudgifterUpdateDTO dto)
        {
            _logger.LogInformation("Updating expense with ID: {VudgiftId} for user with ID: {BrugerId}", id, brugerId);

            var Vudgifter = await _VudgifterRepo.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException("Vudgifter not found.");

            if (Vudgifter.BrugerId != brugerId)
            {
                _logger.LogWarning("Unauthorized update attempt for expense with ID: {VudgiftId} by user with ID: {BrugerId}", id, brugerId);
                throw new UnauthorizedAccessException("Unauthorized.");
            }
            //Check which things to update
            if (dto.Pris.HasValue) Vudgifter.Pris = dto.Pris.Value;
            if (!string.IsNullOrWhiteSpace(dto.Tekst)) Vudgifter.Tekst = dto.Tekst;
            if (dto.Dato.HasValue) Vudgifter.Dato = dto.Dato.Value;

            if (dto.KategoriId.HasValue)
            {
                _logger.LogInformation("Updating category for expense ID: {VudgiftId} to category ID: {KategoriId}", id, dto.KategoriId.Value);
                Vudgifter.Kategori = await _kategoriRepo.GetByIdAsync(dto.KategoriId.Value)
                             ?? throw new KeyNotFoundException("Kategori not found.");
            }
            else if (!string.IsNullOrWhiteSpace(dto.KategoriNavn))
            {
                _logger.LogInformation("Searching for category by name: {KategoriNavn}", dto.KategoriNavn);
                var kategori = await _kategoriRepo.SearchByName(dto.KategoriNavn);

                if (kategori == null)
                {
                    _logger.LogInformation("Category not found. Creating new category: {KategoriNavn}", dto.KategoriNavn);
                    kategori = await _kategoriRepo.NyKategori(dto.KategoriNavn);
                }

                Vudgifter.Kategori = kategori;
            }

            _VudgifterRepo.Update(Vudgifter);
            await _VudgifterRepo.SaveChangesAsync();

            _logger.LogInformation("Successfully updated expense with ID: {VudgiftId}", id);
        }

        // Delete an expense for a user
        public async Task DeleteVudgifter(int brugerId, int id)
        {
            _logger.LogInformation("Deleting expense with ID: {VudgiftId} for user with ID: {BrugerId}", id, brugerId);

            var Vudgifter = await _VudgifterRepo.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException("Vudgifter not found.");

            if (Vudgifter.BrugerId != brugerId)
            {
                _logger.LogWarning("Unauthorized delete attempt for expense with ID: {VudgiftId} by user with ID: {BrugerId}", id, brugerId);
                throw new UnauthorizedAccessException("Unauthorized.");
            }

            _VudgifterRepo.Delete(id);
            await _VudgifterRepo.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted expense with ID: {VudgiftId} for user with ID: {BrugerId}", id, brugerId);
        }
    }
}
