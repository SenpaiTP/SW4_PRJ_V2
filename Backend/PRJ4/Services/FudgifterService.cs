using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.DTOs;
using AutoMapper;

namespace PRJ4.Services
{
    public class FudgifterService : IFudgifterService
    {
        private readonly IFudgifterRepo _fudgifterRepo;
        private readonly IKategoriRepo _kategoriRepo;
        private readonly ILogger<FudgifterService> _logger;
        private readonly IMapper _mapper; 

        public FudgifterService(
            IFudgifterRepo fudgifterRepo,
            IKategoriRepo kategoriRepo,
            ILogger<FudgifterService> logger,
            IMapper mapper)
        {
            _fudgifterRepo = fudgifterRepo;
            _kategoriRepo = kategoriRepo;
            _logger = logger;
            _mapper = mapper; 
        }

        // Get all expenses for a user
        public async Task<IEnumerable<FudgifterResponseDTO>> GetAllByUser(string brugerId)
        {
            var fudgifter = await _fudgifterRepo.GetAllByUserId(brugerId);

            _logger.LogInformation("Found {Count} expenses for user with ID: {BrugerId}", fudgifter.Count(), brugerId);

            // Use AutoMapper to map Fudgifter to FudgifterResponseDTO
            return _mapper.Map<IEnumerable<FudgifterResponseDTO>>(fudgifter);
        }

        // Add a new expense for a user
        public async Task<FudgifterResponseDTO> AddFudgifter(string brugerId, nyFudgifterDTO nydto)
        {

            Kategori kategori;

            if (nydto.KategoriId <= 0)
            {
                _logger.LogInformation("Searching for category by name: {KategoriNavn}", nydto.KategoriNavn);
                kategori = await _kategoriRepo.SearchByName(nydto.KategoriNavn);

                if (kategori == null)
                {
                    _logger.LogInformation("Category not found. Creating new category: {KategoriNavn}", nydto.KategoriNavn);
                    kategori = await _kategoriRepo.NyKategori(nydto.KategoriNavn);
                }
            }
            else
            {
                _logger.LogInformation("Fetching category by ID: {KategoriId}", nydto.KategoriId);
                kategori = await _kategoriRepo.GetByIdAsync(nydto.KategoriId)
                           ?? throw new KeyNotFoundException("Kategori not found.");
            }
            nydto.KategoriId = kategori.KategoriId;
            var nyFudgifter = _mapper.Map<Fudgifter>(nydto);
            // Set additional properties
            nyFudgifter.BrugerId = brugerId;
            nyFudgifter.Kategori = kategori;

            await _fudgifterRepo.AddAsync(nyFudgifter);
            await _fudgifterRepo.SaveChangesAsync();
            // Use AutoMapper to return the DTO
            return _mapper.Map<FudgifterResponseDTO>(nyFudgifter);
        }

        // Update an existing expense for a user
        public async Task UpdateFudgifter(string brugerId, int id,  FudgifterUpdateDTO dto)
        {

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
        }

        // Delete an expense for a user
        public async Task DeleteFudgifter(string brugerId, int id)
        {

            var fudgifter = await _fudgifterRepo.GetByIdAsync(id)
                           ?? throw new KeyNotFoundException("Fudgifter not found.");

            if (fudgifter.BrugerId != brugerId)
            {
                _logger.LogWarning("Unauthorized delete attempt for expense with ID: {FudgiftId} by user with ID: {BrugerId}", id, brugerId);
                throw new UnauthorizedAccessException("Unauthorized.");
            }

            _fudgifterRepo.Delete(id);
            await _fudgifterRepo.SaveChangesAsync();
        }
    }
}
