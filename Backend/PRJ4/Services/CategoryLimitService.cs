using PRJ4.Models;  
using PRJ4.Repositories;
using PRJ4.DTOs;

namespace PRJ4.Services
{
    public class CategoryLimitService: ICategoryLimitService
    {
        private readonly ICategoryLimitRepo _CategoryLimitRepository;

        private readonly IKategoriRepo _CategoryRepository;

        private readonly ILogger<CategoryLimitService> _logger;

        public CategoryLimitService(ICategoryLimitRepo CategoryLimitRepository, IKategoriRepo CategoryRepository, ILogger<CategoryLimitService> logger)
        {
            _CategoryLimitRepository = CategoryLimitRepository;
            _CategoryRepository = CategoryRepository;
            _logger = logger;
        }

        public async Task<List<CategoryLimitResponseDTO>> GetAllCategoryLimits(string userId)
        {
            //Validating parameter
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.");
            }

            // Fetch category limits from repository
            var limitListe = await _CategoryLimitRepository.GetCategoryLimitsForUserAsync(userId);
            if (limitListe == null || !limitListe.Any())
            {
                //Returns an empty list if no limits sat
                return new List<CategoryLimitResponseDTO>();
            }

            _logger.LogInformation($"Successfully fetched category limits for user");

            var limitReturnListe = new List<CategoryLimitResponseDTO>();

            foreach( var limit in limitListe)
            {
                var limitReturn = new CategoryLimitResponseDTO
                {
                    CategoryLimitId = limit.CategoryLimitId,
                    CategoryId = limit.CategoryId,
                    CategoryName = limit.Category.KategoriNavn,
                    Limit = limit.Limit
                };
                limitReturnListe.Add(limitReturn);
            }

            return limitReturnListe;
        }

        public async Task<CategoryLimitResponseDTO> GetByIdCategoryLimits(int CategoryLimitId, string userId)
        {
            //Validating parameter
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(nameof(userId), "User ID cannot be null or empty.");
            }
            if (CategoryLimitId <= 0)
            {
                throw new ArgumentException(nameof(CategoryLimitId), "The provided category limit ID is invalid.");
            }

            // Fetch category limit from repository
            var limit = await _CategoryLimitRepository.GetCategoryLimitForCategoryAsync(CategoryLimitId, userId);
            if (limit == null)
            {
                throw new KeyNotFoundException($"No category limit found for category ID {limit.CategoryId}.");
            }

            // Log success and return DTO
            _logger.LogInformation($"Successfully fetched category limit for category ID: {limit.CategoryId} for user ID", limit.CategoryId);

            return new CategoryLimitResponseDTO
            {
                CategoryLimitId = limit.CategoryLimitId,
                CategoryId = limit.CategoryId,
                CategoryName = limit.Category.KategoriNavn,
                Limit = limit.Limit
            };         
        }

        public async Task<CategoryLimitResponseDTO> AddCategoryLimitAsync(CategoryLimitCreateDTO CategoryLimitDTO, string userId)
        {
            // Check parameters
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException( nameof(userId), "User ID cannot be null or empty.");
            }
            if (CategoryLimitDTO.Limit < 0)
            {
                throw new ArgumentException(nameof(CategoryLimitDTO.Limit), "The limit is out of range");
            }

            var Category = await _CategoryRepository.GetByIdAsync(CategoryLimitDTO.CategoryId);
            if(Category == null) 
            {
                throw new ArgumentException($"The specified category does not exist.");
            }

            
            //Create new CategoryLimit
            var CategoryLimit = new CategoryLimit
            {
                CategoryId = Category.KategoriId,
                Limit = CategoryLimitDTO.Limit,
                BrugerId = userId
            };
            

            //Add and save the budget
            try {
                var createdBudget = await _CategoryLimitRepository.AddAsync(CategoryLimit);
                await _CategoryLimitRepository.SaveChangesAsync();

                _logger.LogInformation($"Successfully created category limit for category ID: {createdBudget.CategoryId} for user");

                //Create CategoryLimit DTO to return
                return new CategoryLimitResponseDTO
                {
                    CategoryLimitId = createdBudget.CategoryLimitId,
                    CategoryId = createdBudget.CategoryId,
                    CategoryName = Category.KategoriNavn,
                    Limit = createdBudget.Limit
                };
            }
            catch(Exception ex) {
                _logger.LogError($"Error while saving category limit: {ex.Message}");
                throw new InvalidOperationException("Could not save the category limit.");

            }
   
        }

        public async Task<CategoryLimitResponseDTO> UpdateCategoryLimitAsync(int CategoryLimitId, string userId, CategoryLimitUpdateDTO CategoryLimitDTO)
        {
            // Validating parameters
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(nameof(userId), "User ID cannot be null or empty.");
            }
            if (CategoryLimitId <= 0)
            {
                throw new ArgumentException(nameof(CategoryLimitId), "The provided category ID is invalid.");
            }
            if (CategoryLimitDTO.Limit < 0)
            {
                throw new ArgumentException(nameof(CategoryLimitDTO.Limit), "The limit is out of range.");
            }

            // Fetch existing category limit from repository
            var limit = await _CategoryLimitRepository.GetCategoryLimitForCategoryAsync(CategoryLimitId, userId);
            if (limit == null)
            {
                throw new KeyNotFoundException($"No category limit found with ID {limit.CategoryId}.");
            }

            // Update the category limit with new values
            limit.Limit = CategoryLimitDTO.Limit;

            // Try to update the category limit
            try
            {
                await _CategoryLimitRepository.SaveChangesAsync();
                _logger.LogInformation($"Successfully updated category limit  with ID{CategoryLimitId}  for user.");
                
                // Return updated DTO
                return new CategoryLimitResponseDTO
                {
                    CategoryLimitId = limit.CategoryLimitId,
                    CategoryId = limit.CategoryId,
                    CategoryName = limit.Category.KategoriNavn,
                    Limit = limit.Limit
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating category limit: {ex.Message}");
                throw new InvalidOperationException("Could not update the category limit.");
            }
        }


        public async Task DeleteCategoryLimitAsync(int CategoryLimitId, string userId)
        {
            // Validating parameters
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(nameof(userId), "User ID cannot be null or empty.");
            }
            if (CategoryLimitId <= 0)
            {
                throw new ArgumentException(nameof(CategoryLimitId), "The provided category limit ID is invalid.");
            }

            // Fetch category limit from repository
            var limit = await _CategoryLimitRepository.GetCategoryLimitForCategoryAsync(CategoryLimitId, userId);
            if (limit == null)
            {
                throw new KeyNotFoundException($"No category limit with ID {CategoryLimitId}.");
            }

            // Try to delete the category limit
            try
            {
                await _CategoryLimitRepository.Delete(CategoryLimitId);
                await _CategoryLimitRepository.SaveChangesAsync();
                _logger.LogInformation($"Successfully deleted category limit with ID: {CategoryLimitId} for user.");
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting category limit: {ex.Message}");
                throw new InvalidOperationException("Could not delete the category limit.");
            }
        }
        
    }
}