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
        private readonly IKategoriRepo _kategoriRepo;
        private readonly ILogger<VudgifterService> _logger;
        private readonly IMapper _mapper;

        public VudgifterService(
            IVudgifter VudgifterRepo,
            IKategoriRepo kategoriRepo,
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
        public async Task<IEnumerable<VudgifterResponseDTO>> GetAllByUser(string brugerId)
        {

            var Vudgifter = await _VudgifterRepo.GetAllByUserId(brugerId);

            _logger.LogInformation("Found {Count} expenses for user with ID: {BrugerId}", Vudgifter.Count(), brugerId);

            // Use AutoMapper to map Vudgifter to VudgifterResponseDTO
            return _mapper.Map<IEnumerable<VudgifterResponseDTO>>(Vudgifter);
        }

        // Add a new expense for a user
        public async Task<VudgifterResponseDTO> AddVudgifter(string brugerId, nyVudgifterDTO dto)
        {

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

            await _VudgifterRepo.AddAsync(nyVudgifter);
            await _VudgifterRepo.SaveChangesAsync();

            // Use AutoMapper to return the DTO
            return _mapper.Map<VudgifterResponseDTO>(nyVudgifter);
        }

        // Update an existing expense for a user
        public async Task UpdateVudgifter(string brugerId, int id, VudgifterUpdateDTO nydto)
        {

            var Vudgifter = await _VudgifterRepo.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException("Vudgifter not found.");

            if (Vudgifter.BrugerId != brugerId)
            {
                _logger.LogWarning("Unauthorized update attempt for expense with ID: {VudgiftId} by user with ID: {BrugerId}", id, brugerId);
                throw new UnauthorizedAccessException("Unauthorized.");
            }
            // Check if the updateDTO is empty
            if (nydto == null || 
                (!nydto.Pris.HasValue && string.IsNullOrWhiteSpace(nydto.Tekst) && !nydto.Dato.HasValue && !nydto.KategoriId.HasValue && string.IsNullOrWhiteSpace(nydto.KategoriNavn)))
            {
                _logger.LogWarning("Update request for expense ID {VudgiftId} is empty or invalid.", id);
                throw new ArgumentException("No valid data provided for update.");
            }

            // Check which properties to update
            if (nydto.Pris.HasValue) Vudgifter.Pris = nydto.Pris.Value;
            if (!string.IsNullOrWhiteSpace(nydto.Tekst)) Vudgifter.Tekst = nydto.Tekst;
            if (nydto.Dato.HasValue) Vudgifter.Dato = nydto.Dato.Value;

            if (nydto.KategoriId.HasValue)
            {
                _logger.LogInformation("Updating category for expense ID: {VudgiftId} to category ID: {KategoriId}", id, nydto.KategoriId.Value);
                Vudgifter.Kategori = await _kategoriRepo.GetByIdAsync(nydto.KategoriId.Value)
                             ?? throw new KeyNotFoundException("Kategori not found.");
            }
            else if (!string.IsNullOrWhiteSpace(nydto.KategoriNavn))
            {
                _logger.LogInformation("Searching for category by name: {KategoriNavn}", nydto.KategoriNavn);
                var kategori = await _kategoriRepo.SearchByName(nydto.KategoriNavn);

                if (kategori == null)
                {
                    _logger.LogInformation("Category not found. Creating new category: {KategoriNavn}", nydto.KategoriNavn);
                    kategori = await _kategoriRepo.NyKategori(nydto.KategoriNavn);
                }

                Vudgifter.Kategori = kategori;
            }

            _VudgifterRepo.Update(Vudgifter);
            await _VudgifterRepo.SaveChangesAsync();
        }

        // Delete an expense for a user
        public async Task DeleteVudgifter(string brugerId, int id)
        {
            var Vudgifter = await _VudgifterRepo.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException("Vudgifter not found.");

            if (Vudgifter.BrugerId != brugerId)
            {
                _logger.LogWarning("Unauthorized delete attempt for expense with ID: {VudgiftId} by user with ID: {BrugerId}", id, brugerId);
                throw new UnauthorizedAccessException("Unauthorized.");
            }

            _VudgifterRepo.Delete(id);
            await _VudgifterRepo.SaveChangesAsync();
        }
    }
}
