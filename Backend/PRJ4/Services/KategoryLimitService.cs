using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;

namespace PRJ4.Services
{
    public class KategoryLimitService: IKategoryLimitService
    {
        private readonly IKategoryLimitRepo _kategoryLimitRepository;

        private readonly IKategoriRepo _kategoryRepository;

         private readonly ILogger<KategoryLimitService> _logger;

        public KategoryLimitService(IKategoryLimitRepo kategoryLimitRepository, IKategoriRepo kategoryRepository, ILogger<KategoryLimitService> logger)
        {
            _kategoryLimitRepository = kategoryLimitRepository;
            _kategoryRepository = kategoryRepository;
            _logger = logger;
        }

        public async Task<List<KategoryLimitResponseDTO>> GetAllKategoryLimits(string userId)
        {
            //Validating parameter
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(nameof(userId), "User ID cannot be null or empty.");
            }

            // Fetch category limits from repository
            var limitListe = await _kategoryLimitRepository.GetKategoryLimitsForUserAsync(userId);
            if (limitListe == null || !limitListe.Any())
            {
                //Returns an empty list if no limits sat
                return new List<KategoryLimitResponseDTO>();
            }

            _logger.LogInformation($"Successfully fetched category limits for user ID: {userId}.", userId);

            var limitReturnListe = new List<KategoryLimitResponseDTO>();

            foreach( var limit in limitListe)
            {
                var limitReturn = new KategoryLimitResponseDTO
                {
                    KategoryLimitId = limit.KategoryLimitId,
                    KategoryId = limit.KategoryId,
                    KategoryName = limit.Kategory.KategoriNavn,
                    Limit = limit.Limit
                };
                limitReturnListe.Add(limitReturn);
            }

            return limitReturnListe;
        }

        public async Task<KategoryLimitResponseDTO> GetByIdKategoryLimits(int kategoryId, string userId)
        {
            //Validating parameter
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(nameof(userId), "User ID cannot be null or empty.");
            }
            if (kategoryId <= 0)
            {
                throw new ArgumentException(nameof(kategoryId), "The provided category ID is invalid.");
            }

            // Fetch category limit from repository
            var limit = await _kategoryLimitRepository.GetKategoryLimitForKategoryAsync(kategoryId, userId);
            if (limit == null)
            {
                throw new KeyNotFoundException($"No category limit found for category ID {kategoryId}.");
            }

            // Log success and return DTO
            _logger.LogInformation($"Successfully fetched category limit for category ID: {kategoryId} for user ID: {userId}.", kategoryId, userId);

            return new KategoryLimitResponseDTO
            {
                KategoryLimitId = limit.KategoryLimitId,
                KategoryId = limit.KategoryId,
                KategoryName = limit.Kategory.KategoriNavn,
                Limit = limit.Limit
            };         
        }

        public async Task<KategoryLimitResponseDTO> AddKategoryLimitAsync(KategoryLimitCreateDTO kategoryLimitDTO, string userId)
        {
            // Check parameters
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException( nameof(userId), "User ID cannot be null or empty.");
            }
            if (kategoryLimitDTO.Limit < 0)
            {
                throw new ArgumentException(nameof(kategoryLimitDTO.Limit), "The limit is out of range");
            }

            var kategory = await _kategoryRepository.GetByIdAsync(kategoryLimitDTO.KategoryId);
            if(kategory == null) 
            {
                throw new ArgumentException($"The specified category does not exist.");
            }

            
            //Create new kategoryLimit
            var kategoryLimit = new KategoryLimit
            {
                KategoryId = kategory.KategoriId,
                Limit = kategoryLimitDTO.Limit,
                BrugerId = userId
            };
            

            //Add and save the budget
            try {
                var createdBudget = await _kategoryLimitRepository.AddAsync(kategoryLimit);
                await _kategoryLimitRepository.SaveChangesAsync();

                _logger.LogInformation($"Successfully created category limit for category ID: {createdBudget.KategoryId} for user ID: {userId}.");

                //Create kategoryLimit DTO to return
                return new KategoryLimitResponseDTO
                {
                    KategoryLimitId = createdBudget.KategoryLimitId,
                    KategoryId = createdBudget.KategoryId,
                    KategoryName = kategory.KategoriNavn,
                    Limit = createdBudget.Limit
                };
            }
            catch(Exception ex) {
                _logger.LogError($"Error while saving category limit: {ex.Message}");
                throw new InvalidOperationException("Could not save the category limit.");

            }
   
        }

        public async Task<KategoryLimitResponseDTO> UpdateKategoryLimitAsync(int kategoryId, string userId, KategoryLimitUpdateDTO kategoryLimitDTO)
        {
            // Validating parameters
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(nameof(userId), "User ID cannot be null or empty.");
            }
            if (kategoryId <= 0)
            {
                throw new ArgumentException(nameof(kategoryId), "The provided category ID is invalid.");
            }
            if (kategoryLimitDTO.Limit < 0)
            {
                throw new ArgumentException(nameof(kategoryLimitDTO.Limit), "The limit is out of range.");
            }

            // Fetch existing category limit from repository
            var limit = await _kategoryLimitRepository.GetKategoryLimitForKategoryAsync(kategoryId, userId);
            if (limit == null)
            {
                throw new KeyNotFoundException($"No category limit found for category ID {kategoryId}.");
            }

            // Update the category limit with new values
            limit.Limit = kategoryLimitDTO.Limit;

            // Try to update the category limit
            try
            {
                await _kategoryLimitRepository.SaveChangesAsync();
                _logger.LogInformation($"Successfully updated category limit for category ID: {kategoryId} for user ID: {userId}.");
                
                // Return updated DTO
                return new KategoryLimitResponseDTO
                {
                    KategoryLimitId = limit.KategoryLimitId,
                    KategoryId = limit.KategoryId,
                    KategoryName = limit.Kategory.KategoriNavn,
                    Limit = limit.Limit
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating category limit: {ex.Message}");
                throw new InvalidOperationException("Could not update the category limit.");
            }
        }


        public async Task DeleteKategoryLimitAsync(int kategoryId, string userId)
        {
            // Validating parameters
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(nameof(userId), "User ID cannot be null or empty.");
            }
            if (kategoryId <= 0)
            {
                throw new ArgumentException(nameof(kategoryId), "The provided category ID is invalid.");
            }

            // Fetch category limit from repository
            var limit = await _kategoryLimitRepository.GetKategoryLimitForKategoryAsync(kategoryId, userId);
            if (limit == null)
            {
                throw new KeyNotFoundException($"No category limit found for category ID {kategoryId}.");
            }

            // Try to delete the category limit
            try
            {
                await _kategoryLimitRepository.Delete(limit.KategoryLimitId);
                await _kategoryLimitRepository.SaveChangesAsync();
                _logger.LogInformation($"Successfully deleted category limit for category ID: {kategoryId} for user ID: {userId}.");
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting category limit: {ex.Message}");
                throw new InvalidOperationException("Could not delete the category limit.");
            }
        }
        
    }
}