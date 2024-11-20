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

        public VudgifterService(IVudgifter VudgifterRepo, IKategori kategoriRepo, IBrugerRepo brugerRepo)
        {
            _VudgifterRepo = VudgifterRepo;
            _kategoriRepo = kategoriRepo;
            _brugerRepo = brugerRepo;
        }

        public async Task<IEnumerable<VudgifterResponseDTO>> GetAllByUser(int brugerId)
        {
            var Vudgifter = await _VudgifterRepo.GetAllByUserId(brugerId);
            return Vudgifter.Select(f => new VudgifterResponseDTO
            {
                VudgiftId = f.VudgiftId,
                Pris = f.Pris,
                Tekst = f.Tekst,
                KategoriNavn = f.Kategori?.Navn,
                Dato = f.Dato
            });
        }

        public async Task<VudgifterResponseDTO> AddVudgifter(int brugerId, nyVudgifterDTO dto)
        {
            Bruger bruger = await _brugerRepo.GetByIdAsync(brugerId);
            if (bruger == null) throw new KeyNotFoundException("Bruger not found.");

            Kategori kategori;

            if (dto.KategoriId <= 0)
            {
                // Search for Kategori by name
                kategori = await _kategoriRepo.SearchByName(dto.KategoriNavn);
                Console.WriteLine($"{kategori.Navn}, {kategori.KategoriId}, Service Before create new");

                // If Kategori not found, create a new one
                if (kategori == null)
                {
                     Console.WriteLine($"{kategori.Navn}, {kategori.KategoriId}, Service In create new");
                    kategori = await _kategoriRepo.NyKategori(dto.KategoriNavn);
                }
            }
            else
            {
                // Find Kategori by ID or throw an exception
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

            return new VudgifterResponseDTO
            {
                VudgiftId = nyVudgifter.VudgiftId,
                Pris = nyVudgifter.Pris,
                Tekst = nyVudgifter.Tekst,
                Dato = nyVudgifter.Dato,
                KategoriNavn = kategori.Navn
            };
        }

        public async Task UpdateVudgifter(int id, int brugerId, VudgifterUpdateDTO dto)
        {
            // Get the existing Vudgifter
            var Vudgifter = await _VudgifterRepo.GetByIdAsync(id) 
                            ?? throw new KeyNotFoundException("Vudgifter not found.");

            // Check if the logged-in user matches the one who created the Vudgifter
            if (Vudgifter.BrugerId != brugerId)
                throw new UnauthorizedAccessException("Unauthorized.");

            // Update fields if provided
            if (dto.Pris.HasValue) Vudgifter.Pris = dto.Pris.Value;
            if (!string.IsNullOrWhiteSpace(dto.Tekst)) Vudgifter.Tekst = dto.Tekst;
            if (dto.Dato.HasValue) Vudgifter.Dato = dto.Dato.Value;

            // Handle Kategori (either by ID or by name)
            if (dto.KategoriId.HasValue)
            {
                // Check if KategoriId is valid
                Vudgifter.Kategori = await _kategoriRepo.GetByIdAsync(dto.KategoriId.Value)
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
                Vudgifter.Kategori = kategori;
            }

            // Update the Vudgifter record
            _VudgifterRepo.Update(Vudgifter);
            await _VudgifterRepo.SaveChangesAsync();
        }


        public async Task DeleteVudgifter(int brugerId, int id)
        {
            var Vudgifter = await _VudgifterRepo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Vudgifter not found.");
            if (Vudgifter.BrugerId != brugerId) throw new UnauthorizedAccessException("Unauthorized.");

            _VudgifterRepo.Delete(id);
            await _VudgifterRepo.SaveChangesAsync();
        }
    }

}