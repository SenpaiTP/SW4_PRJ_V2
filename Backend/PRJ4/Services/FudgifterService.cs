using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;



namespace PRJ4.Services
{
    public class FudgifterService : IFudgifterService
    {
        private readonly IFudgifter _fudgifterRepo;
        private readonly IBrugerRepo _brugerRepo;
        private readonly IKategori _kategoriRepo;

        public FudgifterService(IFudgifter fudgifterRepo, IKategori kategoriRepo, IBrugerRepo brugerRepo)
        {
            _fudgifterRepo = fudgifterRepo;
            _kategoriRepo = kategoriRepo;
            _brugerRepo = brugerRepo;
        }

        public async Task<IEnumerable<FudgifterResponseDTO>> GetAllByUser(int brugerId)
        {
            var fudgifter = await _fudgifterRepo.GetAllByUserId(brugerId);
            return fudgifter.Select(f => new FudgifterResponseDTO
            {
                FudgiftId = f.FudgiftId,
                Pris = f.Pris,
                Tekst = f.Tekst,
                KategoriNavn = f.Kategori?.Navn,
                Dato = f.Dato
            });
        }

        public async Task<FudgifterResponseDTO> AddFudgifter(int brugerId, nyFudgifterDTO dto)
        {
            Bruger bruger = await _brugerRepo.GetByIdAsync(brugerId);
            if (bruger == null) throw new KeyNotFoundException("Bruger not found.");
            
            Kategori kategori;

            if (dto.KategoriId <= 0)
            {
                // Search for Kategori by name
                kategori = await _kategoriRepo.SearchByName(dto.KategoriNavn);

                // If Kategori not found, create a new one
                if (kategori == null)
                {
                    kategori = await _kategoriRepo.NyKategori(dto.KategoriNavn);
                }
            }
            else
            {
                // Find Kategori by ID or throw an exception
                kategori = await _kategoriRepo.GetByIdAsync(dto.KategoriId)
                        ?? throw new KeyNotFoundException("Kategori not found.");
            }

            var nyFudgifter = new Fudgifter
            {
                Pris = dto.Pris,
                Tekst = dto.Tekst,
                Dato = dto.Dato,
                KategoriId = kategori.KategoriId,
                BrugerId = brugerId,
                Kategori = kategori,
                Bruger = bruger
            };

            await _fudgifterRepo.AddAsync(nyFudgifter);
            await _fudgifterRepo.SaveChangesAsync();

            return new FudgifterResponseDTO
            {
                FudgiftId = nyFudgifter.FudgiftId,
                Pris = nyFudgifter.Pris,
                Tekst = nyFudgifter.Tekst,
                Dato = nyFudgifter.Dato,
                KategoriNavn = kategori.Navn
            };
        }

        public async Task UpdateFudgifter(int id, int brugerId, FudgifterUpdateDTO dto)
        {
            // Get the existing Fudgifter
            var Fudgifter = await _fudgifterRepo.GetByIdAsync(id) 
                            ?? throw new KeyNotFoundException("Fudgifter not found.");

            // Check if the logged-in user matches the one who created the Fudgifter
            if (Fudgifter.BrugerId != brugerId)
                throw new UnauthorizedAccessException("Unauthorized.");

            // Update fields if provided
            if (dto.Pris.HasValue) Fudgifter.Pris = dto.Pris.Value;
            if (!string.IsNullOrWhiteSpace(dto.Tekst)) Fudgifter.Tekst = dto.Tekst;
            if (dto.Dato.HasValue) Fudgifter.Dato = dto.Dato.Value;

            // Handle Kategori (either by ID or by name)
            if (dto.KategoriId.HasValue)
            {
                // Check if KategoriId is valid
                Fudgifter.Kategori = await _kategoriRepo.GetByIdAsync(dto.KategoriId.Value)
                    ?? throw new KeyNotFoundException("Kategori not found.");
            }
            else if (!string.IsNullOrWhiteSpace(dto.KategoriNavn))
            {
                // Search for the category by name
                var kategori = await _kategoriRepo.SearchByName(dto.KategoriNavn);

                // If Kategori not found, create a new one
                if (kategori == null)
                {
                    kategori = await _kategoriRepo.NyKategori(dto.KategoriNavn);
                }

                // Assign the found or newly created category
                Fudgifter.Kategori = kategori;
            }

            // Update the Fudgifter record
            _fudgifterRepo.Update(Fudgifter);
            await _fudgifterRepo.SaveChangesAsync();
        }

        public async Task DeleteFudgifter(int brugerId, int id)
        {
            var fudgifter = await _fudgifterRepo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Fudgifter not found.");
            if (fudgifter.BrugerId != brugerId) throw new UnauthorizedAccessException("Unauthorized.");

            _fudgifterRepo.Delete(id);
            await _fudgifterRepo.SaveChangesAsync();
        }
    }

}