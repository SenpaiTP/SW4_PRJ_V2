using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;

namespace PRJ4.Services
{
    public class VudgifterService : IVudgifterService
    {
        private readonly IVudgifter _VudgifterRepo;
        private readonly IBrugerRepo _brugerRepo;
        private readonly IKategori _kategoriRepo;
        private readonly ILogger<VudgifterService> _logger;

        public VudgifterService(IVudgifter VudgifterRepo, IKategori kategoriRepo, IBrugerRepo brugerRepo, ILogger<VudgifterService> logger)
        {
            _VudgifterRepo = VudgifterRepo;
            _kategoriRepo = kategoriRepo;
            _brugerRepo = brugerRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<VudgifterResponseDTO>> GetAllByUser(int brugerId)
        {
            try
            {
                _logger.LogInformation("Fetching all variable expenses for user {BrugerId}", brugerId);
                var Vudgifter = await _VudgifterRepo.GetAllByUserId(brugerId);
                _logger.LogInformation("Fetched {Count} variable expenses for user {BrugerId}", Vudgifter.Count(), brugerId);

                return Vudgifter.Select(f => new VudgifterResponseDTO
                {
                    VudgiftId = f.VudgiftId,
                    Pris = f.Pris,
                    Tekst = f.Tekst,
                    KategoriNavn = f.Kategori?.Navn,
                    Dato = f.Dato
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching variable expenses for user {BrugerId}", brugerId);
                throw;
            }
        }

        public async Task<VudgifterResponseDTO> AddVudgifter(int brugerId, nyVudgifterDTO dto)
        {
            try
            {
                _logger.LogInformation("Adding new variable expense for user {BrugerId} with data {@Dto}", brugerId, dto);
                var bruger = await _brugerRepo.GetByIdAsync(brugerId) 
                            ?? throw new KeyNotFoundException("Bruger not found.");
                
                Kategori kategori;
                if (dto.KategoriId <= 0)
                {
                    kategori = await _kategoriRepo.SearchByName(dto.KategoriNavn);
                    if (kategori == null)
                    {
                        _logger.LogInformation("Creating new category '{KategoriNavn}' for user {BrugerId}", dto.KategoriNavn, brugerId);
                        kategori = await _kategoriRepo.NyKategori(dto.KategoriNavn);
                    }
                }
                else
                {
                    kategori = await _kategoriRepo.GetByIdAsync(dto.KategoriId) 
                              ?? throw new KeyNotFoundException("Kategori not found.");
                }

                var nyVudgifter = new Vudgifter
                {
                    Pris = dto.Pris,
                    Tekst = dto.Tekst,
                    Dato = dto.Dato,
                    KategoriId = kategori.KategoriId,
                    BrugerId = brugerId,
                    Kategori = kategori,
                    Bruger = bruger
                };

                await _VudgifterRepo.AddAsync(nyVudgifter);
                await _VudgifterRepo.SaveChangesAsync();

                _logger.LogInformation("Added variable expense {VudgiftId} for user {BrugerId}", nyVudgifter.VudgiftId, brugerId);
                return new VudgifterResponseDTO
                {
                    VudgiftId = nyVudgifter.VudgiftId,
                    Pris = nyVudgifter.Pris,
                    Tekst = nyVudgifter.Tekst,
                    Dato = nyVudgifter.Dato,
                    KategoriNavn = kategori.Navn
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding variable expense for user {BrugerId}", brugerId);
                throw;
            }
        }

        public async Task UpdateVudgifter(int id, int brugerId, VudgifterUpdateDTO dto)
        {
            try
            {
                _logger.LogInformation("Updating variable expense {VudgiftId} for user {BrugerId} with data {@Dto}", id, brugerId, dto);
                var Vudgifter = await _VudgifterRepo.GetByIdAsync(id) 
                                ?? throw new KeyNotFoundException("Vudgifter not found.");

                if (Vudgifter.BrugerId != brugerId)
                    throw new UnauthorizedAccessException("Unauthorized.");

                if (dto.Pris.HasValue) Vudgifter.Pris = dto.Pris.Value;
                if (!string.IsNullOrWhiteSpace(dto.Tekst)) Vudgifter.Tekst = dto.Tekst;
                if (dto.Dato.HasValue) Vudgifter.Dato = dto.Dato.Value;

                if (dto.KategoriId.HasValue)
                {
                    Vudgifter.Kategori = await _kategoriRepo.GetByIdAsync(dto.KategoriId.Value) 
                                         ?? throw new KeyNotFoundException("Kategori not found.");
                }
                else if (!string.IsNullOrWhiteSpace(dto.KategoriNavn))
                {
                    var kategori = await _kategoriRepo.SearchByName(dto.KategoriNavn) 
                                  ?? await _kategoriRepo.NyKategori(dto.KategoriNavn);
                    Vudgifter.Kategori = kategori;
                }

                _VudgifterRepo.Update(Vudgifter);
                await _VudgifterRepo.SaveChangesAsync();

                _logger.LogInformation("Updated variable expense {VudgiftId} for user {BrugerId}", id, brugerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating variable expense {VudgiftId} for user {BrugerId}", id, brugerId);
                throw;
            }
        }

        public async Task DeleteVudgifter(int brugerId, int id)
        {
            try
            {
                _logger.LogInformation("Deleting variable expense {VudgiftId} for user {BrugerId}", id, brugerId);
                var Vudgifter = await _VudgifterRepo.GetByIdAsync(id) 
                                ?? throw new KeyNotFoundException("Vudgifter not found.");

                if (Vudgifter.BrugerId != brugerId)
                    throw new UnauthorizedAccessException("Unauthorized.");

                _VudgifterRepo.Delete(id);
                await _VudgifterRepo.SaveChangesAsync();

                _logger.LogInformation("Deleted variable expense {VudgiftId} for user {BrugerId}", id, brugerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting variable expense {VudgiftId} for user {BrugerId}", id, brugerId);
                throw;
            }
        }
    }
}
