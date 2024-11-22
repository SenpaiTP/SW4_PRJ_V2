using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.DTOs;
using AutoMapper;

namespace PRJ4.Services
{
    public class FudgifterService : IFudgifterService
    {
        private readonly IFudgifter _fudgifterRepo;
        private readonly IBrugerRepo _brugerRepo;
        private readonly IKategori _kategoriRepo;
        private readonly ILogger<FudgifterService> _logger;
        private readonly IMapper _mapper; 

        public FudgifterService(
            IFudgifter fudgifterRepo,
            IKategori kategoriRepo,
            IBrugerRepo brugerRepo,
            ILogger<FudgifterService> logger,
            IMapper mapper)
        {
            _fudgifterRepo = fudgifterRepo;
            _kategoriRepo = kategoriRepo;
            _brugerRepo = brugerRepo;
            _logger = logger;
            _mapper = mapper; 
        }

        // Get all expenses for a user
        public async Task<IEnumerable<FudgifterResponseDTO>> GetAllByUser(int brugerId)
        {
            _logger.LogInformation("Fetching all expenses for user with ID: {BrugerId}", brugerId);

            var fudgifter = await _fudgifterRepo.GetAllByUserId(brugerId);

            _logger.LogInformation("Found {Count} expenses for user with ID: {BrugerId}", fudgifter.Count(), brugerId);

            // Use AutoMapper to map Fudgifter to FudgifterResponseDTO
            return _mapper.Map<IEnumerable<FudgifterResponseDTO>>(fudgifter);
        }

        // Add a new expense for a user
        public async Task<FudgifterResponseDTO> AddFudgifter(int brugerId, nyFudgifterDTO dto)
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
            var nyFudgifter = _mapper.Map<Fudgifter>(dto);
            // Set additional properties
            nyFudgifter.BrugerId = brugerId;
            nyFudgifter.Kategori = kategori;
            nyFudgifter.Bruger = bruger;

            await _fudgifterRepo.AddAsync(nyFudgifter);
            await _fudgifterRepo.SaveChangesAsync();

            _logger.LogInformation("Successfully added expense with ID: {FudgiftId} for user with ID: {BrugerId}", nyFudgifter.FudgiftId, brugerId);

            // Use AutoMapper to return the DTO
            return _mapper.Map<FudgifterResponseDTO>(nyFudgifter);
        }

        // Update an existing expense for a user
        public async Task UpdateFudgifter(int id, int brugerId, FudgifterUpdateDTO dto)
        {
            _logger.LogInformation("Updating expense with ID: {FudgiftId} for user with ID: {BrugerId}", id, brugerId);

            var fudgifter = await _fudgifterRepo.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException("Fudgifter not found.");

            if (fudgifter.BrugerId != brugerId)
            {
                _logger.LogWarning("Unauthorized update attempt for expense with ID: {FudgiftId} by user with ID: {BrugerId}", id, brugerId);
                throw new UnauthorizedAccessException("Unauthorized.");
            }
            //Check which things to update
            if (dto.Pris.HasValue) fudgifter.Pris = dto.Pris.Value;
            if (!string.IsNullOrWhiteSpace(dto.Tekst)) fudgifter.Tekst = dto.Tekst;
            if (dto.Dato.HasValue) fudgifter.Dato = dto.Dato.Value;

            if (dto.KategoriId.HasValue)
            {
                _logger.LogInformation("Updating category for expense ID: {FudgiftId} to category ID: {KategoriId}", id, dto.KategoriId.Value);
                fudgifter.Kategori = await _kategoriRepo.GetByIdAsync(dto.KategoriId.Value)
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

                fudgifter.Kategori = kategori;
            }

            _fudgifterRepo.Update(fudgifter);
            await _fudgifterRepo.SaveChangesAsync();

            _logger.LogInformation("Successfully updated expense with ID: {FudgiftId}", id);
        }

        // Delete an expense for a user
        public async Task DeleteFudgifter(int brugerId, int id)
        {
            _logger.LogInformation("Deleting expense with ID: {FudgiftId} for user with ID: {BrugerId}", id, brugerId);

            var fudgifter = await _fudgifterRepo.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException("Fudgifter not found.");

            if (fudgifter.BrugerId != brugerId)
            {
                _logger.LogWarning("Unauthorized delete attempt for expense with ID: {FudgiftId} by user with ID: {BrugerId}", id, brugerId);
                throw new UnauthorizedAccessException("Unauthorized.");
            }

            _fudgifterRepo.Delete(id);
            await _fudgifterRepo.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted expense with ID: {FudgiftId} for user with ID: {BrugerId}", id, brugerId);
        }
    }
}
