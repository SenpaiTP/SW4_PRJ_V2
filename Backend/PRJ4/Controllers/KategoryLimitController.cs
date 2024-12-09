using PRJ4.Models;  
using Microsoft.AspNetCore.Mvc;
using PRJ4.Services;
using Microsoft.AspNetCore.Authorization;

namespace PRJ4.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class KategoryLimitController : ControllerBase
{
    private readonly IKategoryLimitService _kategoryLimitService;
    private readonly ILogger<KategoryLimitController> _logger;
    public KategoryLimitController(IKategoryLimitService kategoryLimitService, ILogger<KategoryLimitController> logger)
    {
        _kategoryLimitService = kategoryLimitService;
        _logger = logger; 
    }

    // Method to extract user ID from claims
    private string GetUserId() 
    {
        var claims = Request.HttpContext.User.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last() == "nameidentifier");
        if (userIdClaim == null)
        {
            return null;
        }
        return userIdClaim.Value;
    }  

    /// GET: api/KategoryLimit
    // Fetch all category limits for the authenticated user
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        // Get id for user
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID is missing.");
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to get all category limits
        try
        {
            _logger.LogInformation($"Fetching all kategory limits for user with ID: {userId}");
            var kategoryLimit = await _kategoryLimitService.GetAllKategoryLimits(userId);
            if (kategoryLimit == null || !kategoryLimit.Any())
            {
                _logger.LogInformation($"No category limits found for user with id {userId}");
                return NotFound(new { Message = "No category limits found for this user." });
            }
            return Ok(kategoryLimit); // Returns status code 200 with list of category limits
        }

         // Catching exceptions
        catch(ArgumentException ex)
        {
            // Returns a 400 Bad Request for invalid arguments
            _logger.LogError($"Invalid data while getting category limit for user with id {userId}: {ex.Message}");
            return BadRequest(new { Message = ex.Message });

        }
        catch (Exception ex)
        {
             // Returns a generic 500 error without technical details
            _logger.LogError($"Error getting category limits for user with id {userId}: {ex.Message}");
            return StatusCode(500, new { Message = "An unexpected error occurred."});
        }
    }  
  

    // GET: api/KategoryLimit/{id}
    // Fetch a specific category limit by its ID for the authenticated user
    [HttpGet("{id}")]
    public async Task<ActionResult<KategoryLimitResponseDTO>> GetById(int id)
    {
        // get id for user
        var userId = GetUserId();
        if (userId == null)
        {
            // Return error
            _logger.LogWarning("User ID is missing.");
            return Unauthorized(new { Message = "User not authenticated" });
        }

        //Try to get category limit by id
        try
        {
             _logger.LogInformation($"Fetching all kategory limits with ID: {id} for user with ID: {userId}");
            var kategoryLimit = await _kategoryLimitService.GetByIdKategoryLimits(id, userId);
            return Ok(kategoryLimit); // Returns statuskode and a list of all category limits
        }

        // Catching exceptions
        catch(ArgumentException ex)
        {
            // Returns a 400 Bad Request for invalid arguments
            _logger.LogError($"Invalid data while getting category limit for user with id {userId}: {ex.Message}");
            return BadRequest(new { Message = ex.Message });

        }
        catch (KeyNotFoundException ex)
        {
            // Return 404 Not Found for missing category limit
            _logger.LogWarning($"Category limit not found for user with ID {userId}: {ex.Message}");
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
             // Returns a generic 500 error without technical details
            _logger.LogError($"Error getting category limit for user with id {userId}: {ex.Message}");
            return StatusCode(500, new { Message = "An unexpected error occurred."});
        }
    }  

    // POST: api/KategoryLimit
    // Add a new category limit for the authenticated user
    [HttpPost]  
    public async Task<ActionResult<KategoryLimitResponseDTO>> AddKategoryLimit(KategoryLimitCreateDTO limitDTO) 
    {
        // Get id for user
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID is missing.");
            return Unauthorized(new { Message = "User not authenticated" });
        }

        // Try to add new category limit
        try
        {
           _logger.LogInformation($"Creating a category limit for category with ID: {limitDTO.KategoryId} for user with ID: {userId}");
            var kategoryLimit = await _kategoryLimitService.AddKategoryLimitAsync(limitDTO, userId); 
            return CreatedAtAction(nameof(GetById), new { id = kategoryLimit.KategoryLimitId }, kategoryLimit); // Returns statuskode a list of all category limits
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid data while creating category limit for user with id {userId}: {ex.Message}");
            return BadRequest(new { Message = ex.Message }); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating category limit for user with id {userId}: {ex.Message}");
            return StatusCode(500, new { Message = "An internal server error occurred." }); // More generic for unexpected errors
        }
    }
    // PUT: api/KategoryLimit/{id}
    // Update an existing category limit for the authenticated user
    [HttpPut("{id}")]
    public async Task<ActionResult<KategoryLimitResponseDTO>> UpdateKategoryLimit(int id, KategoryLimitUpdateDTO limitDTO)
    {
        // Get id for user
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID is missing.");
            return Unauthorized(new { Message = "User not authenticated" });
        }

        // Try to update category limit
        try
        {
            _logger.LogInformation($"Updating category limit for category with ID: {id} for user with ID: {userId}");
            var updatedLimit = await _kategoryLimitService.UpdateKategoryLimitAsync(id, userId, limitDTO);
            return Ok(updatedLimit); // Returns the updated category limit
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid data while updating category limit for user with id {userId}: {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Category limit not found for user with id {userId}: {ex.Message}");
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating category limit for user with id {userId}: {ex.Message}");
            return StatusCode(500, new { Message = "An internal server error occurred." });
        }
    }

    // DELETE: api/KategoryLimit/{id}
    // Delete an existing category limit for the authenticated user
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteKategoryLimit(int id)
    {
        // Get id for user
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID is missing.");
            return Unauthorized(new { Message = "User not authenticated" });
        }

        // Try to delete category limit
        try
        {
            _logger.LogInformation($"Deleting category limit for category with ID: {id} for user with ID: {userId}");
            await _kategoryLimitService.DeleteKategoryLimitAsync(id, userId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid data while deleting category limit for user with id {userId}: {ex.Message}");
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Category limit not found for user with id {userId}: {ex.Message}");
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting category limit for user with id {userId}: {ex.Message}");
            return StatusCode(500, new { Message = "An internal server error occurred." });
        }
    }


}