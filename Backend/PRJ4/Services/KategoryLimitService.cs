using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;

namespace PRJ4.Services
{
    public class KategoryLimitService: IKategoryLimitService
    {
        private readonly IKategoryLimitRepo _kategoryLimitRepository;

        private readonly IKategoriRepo _kategoryRepository;

        public KategoryLimitService(IKategoryLimitRepo kategoryLimitRepository, IKategoriRepo kategoryRepository)
        {
            _kategoryLimitRepository = kategoryLimitRepository;
            _kategoryRepository = kategoryRepository;
        }

        //  public async Task<KategoryLimitResponseDTO> GetByIdKategoryLimitAsync(int id, string userId)
        // {
        //     //Check if kategoryLimit with "id" exists and write error message if not.
        //     var kategoryLimit = await _kategoryLimitRepository.GetKategoryLimitsForUserAsync(userId);
        //     if (kategoryLimit == null)
        //     {
        //         throw new KeyNotFoundException($"Kategorylimit with id {id} not found.");
        //     }

        //     //Check if kategory with foreign key "KategoryId" exists and write error message if not.
        //     var kategory = await _kategoryRepository.GetByIdAsync(kategoryLimit.KategoryId);
        //     if (kategory == null)
        //     {
        //         throw new KeyNotFoundException($"Kategory with id {kategoryLimit.KategoryId} not found.");
        //     }

        //     //Create kategoryLimit DTO to return
        //     var budgetReturn = new KategoryLimitResponseDTO
        //     {
        //         KategoryName = kategory.KategoriNavn,
        //         Limit = kategoryLimit.Limit
        //     };
        //     return budgetReturn;
        // }

        public async Task<List<KategoryLimitResponseDTO>> GetAllKategoryLimits(string userId)
        {
            var limitListe = await _kategoryLimitRepository.GetKategoryLimitsForUserAsync(userId);
            if (limitListe == null || !limitListe.Any())
            {
                throw new Exception($"No limits on categories found for user with ID {userId}");
            }

            var limitReturnListe = new List<KategoryLimitResponseDTO>();

            foreach( var limit in limitReturnListe)
            {
                var limitReturn = new KategoryLimitResponseDTO
                {
                    KategoryId = limit.KategoryId,
                    KategoryName = limit.KategoryName,
                    Limit = limit.Limit
                };
                limitReturnListe.Add(limitReturn);
            }

            return limitReturnListe;
        }

        public async Task<KategoryLimitResponseDTO> GetByIdKategoryLimits(int kategoryId, string userId)
        {
            //Validating parameter
            if (kategoryId <= 0)
            {
                throw new ArgumentException("Invalid kategory ID.", nameof(kategoryId));
            }

            var limit = await _kategoryLimitRepository.GetKategoryLimitForKategoryAsync(kategoryId, userId);
            if (limit == null)
            {
                throw new Exception($"No limits on category with id {kategoryId} found for user with ID {userId}");
            }

            var limitReturn = new KategoryLimitResponseDTO
            {
                KategoryId = limit.KategoryId,
                KategoryName = limit.Kategory.KategoriNavn,
                Limit = limit.Limit

            };

            return limitReturn;            
        }

        public async Task<KategoryLimitResponseDTO> AddKategoryLimitAsync(KategoryLimitCreateDTO kategoryLimitDTO, string userId)
        {
            //Check if kategory exists and write error message if not.
            var kategory = await _kategoryRepository.GetByIdAsync(kategoryLimitDTO.KategoryId);
            
            if(kategory == null) 
            {
                throw new ArgumentException($"The category: {kategory.KategoriNavn} does not exist.");
            }

            //Create new kategoryLimit
            var budget = new KategoryLimit
            {
                KategoryId = kategory.KategoriId,
                Limit = kategoryLimitDTO.Limit,
                BrugerId = userId

            };

            //Add and save the budget
            var createdBudget = await _kategoryLimitRepository.AddAsync(budget);
            await _kategoryLimitRepository.SaveChangesAsync();

            //Create kategoryLimit DTO to return
            var createdLimitDTO = new KategoryLimitResponseDTO
            {
                KategoryName = kategory.KategoriNavn,
                Limit = createdBudget.Limit,
     
            };
            return createdLimitDTO;
        }
        // public async Task<KategoryLimitResponseDTO> AddKategoryLimitAsync(KategoryLimitReturnDTO limitDTO)
        // {
        //     //Check if kategory exists and write error message if not.
        //     var kategory = await _kategoryRepository.GetByIdAsync(limitDTO.KategoryId);
            
        //     if(kategory == null) {throw new ArgumentException($"The category: {kategory.KategoriNavn} does not exist.");}
            
        //     //Create new kategoryLimit
        //     var budget = new KategoryLimit
        //     {
        //         KategoryId = limitDTO.KategoryId,
        //         Limit = limitDTO.Limit
        //     };

        //     //Add and save the budget
        //     var createdBudget = await _kategoryLimitRepository.AddAsync(budget);
        //     await _kategoryLimitRepository.SaveChangesAsync();


        //     //Create kategoryLimit DTO to return
        //     var createdBudgetDTO = new KategoryLimitResponseDTO
        //     {
        //         KategoryName = kategory.KategoriNavn,
        //         Limit = createdBudget.Limit,
     
        //     };
        //     return createdBudgetDTO;
        // }

    }
}