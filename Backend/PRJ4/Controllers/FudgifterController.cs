using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Services;
//Usings
//For the controller we only need to know its Service, The DTO's as input, Getting JWT identity 

namespace PRJ4.Controllers
{
    //Definings for a controller class
    [ApiController]
    [Route("api/[controller]")] //Defines route for all endpoints : localhost:<PORT>/Api/Fudgifter/Method
    [Authorize] //Allow only used with correct identity (JWT claim)
    public class FudgifterController : ControllerBase //Inheritance from baseController, also name of controller, which is used in the route part, but Controller is removed from the name thx to ASP.NET
    {
        private readonly IFudgifterService _fudgifterService; //Service reference need for business logic
        private readonly ILogger<FudgifterController> _logger; //Logging reference

        public FudgifterController(IFudgifterService fudgifterService, ILogger<FudgifterController> logger)
        {
            _fudgifterService = fudgifterService;
            _logger = logger;
        }

        private string GetUserId() //Generic function to get nameidentifier, used by all method that need to know user-id
        {
            var claims = Request.HttpContext.User.Claims; //Getting HttpContext of request, then getting user part of httpContext, and 
            var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last() == "nameidentifier");
            if(userIdClaim.Value != null)
            {
                return userIdClaim.Value; //Returning the value of the userIdClaim if found
            }
            return null; //Returning null if no userIdClaim is found
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FudgifterResponseDTO>>> GetAllByUser() //Gets info for all users Faste udgifter, from a ResposneDTO (From client/Frontend)
        {
            try
            {
                string brugerId = GetUserId();//security check
                _logger.LogInformation("Fetching all fixed expenses for user {BrugerId}. {Method}", brugerId, HttpContext.Request.Method);//Always save Request Method also for all future instances

                var fudgifter = await _fudgifterService.GetAllByUser(brugerId);//Using FudgifterService, 
                //(Just middlelayer/busniess logic) get all from database and return as a responseDTO's
                //fudgifter is a IEnumarable of so it can be searched through (Could be reduced to a smaller variable type maybe )

                _logger.LogInformation("Successfully retrieved {Count} fixed expenses for user {BrugerId}. {Method}", fudgifter.Count(), brugerId, HttpContext.Request.Method);
                return Ok(fudgifter); //Returning a successful response with the fetched data
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching fixed expenses for user: {Message}.", ex.Message); //Log error if exception occurs
                return BadRequest(ex.Message); //Returning bad request with the error message
            }
        }

        [HttpPost]
        public async Task<ActionResult<FudgifterResponseDTO>> Add(nyFudgifterDTO fudgifter)
        {
            try
            {
                string brugerId = GetUserId(); //Fetching user ID for security check
                _logger.LogInformation("Adding a new fixed expense for user {BrugerId}. {Method}", brugerId, HttpContext.Request.Method); //Logging the action

                var response = await _fudgifterService.AddFudgifter(brugerId, fudgifter); //Calling the service to add the expense

                _logger.LogInformation("Successfully added fixed expense {FudgiftId} for user {BrugerId}. {Method}", response.FudgiftId, brugerId, HttpContext.Request.Method); //Logging the success
                return CreatedAtAction(nameof(Add), new { id = response.FudgiftId }, response); //Returning the created expense with the new ID
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding fixed expense: {Message}.", ex.Message); //Logging the error if exception occurs
                return BadRequest(ex.Message); //Returning bad request with the error message
            }
        }

        [HttpPut("opdater/{id}")] //When using [FromBody], we only get the data from http request, since its a complex object.
        public async Task<IActionResult> Update(int id,[FromBody] FudgifterUpdateDTO updateDTO) // the updateDTO only need id, brugeriD, and atleast one thing that need to be updated
        {
            try
            {
                string brugerId = GetUserId(); //Fetching user ID for security check
                _logger.LogInformation("Updating fixed expense {Id} for user {BrugerId} with data: {@UpdateDTO}. {Method}", id, brugerId, updateDTO, HttpContext.Request.Method); //Logging the action

                await _fudgifterService.UpdateFudgifter(brugerId, id, updateDTO); //Calling the service to update the expense, here the service handles the updateDTO; where everything about the specific fast udgift could be changed to only one thing being changed

                _logger.LogInformation("Successfully updated fixed expense {Id} for user {BrugerId}. {Method}", id, brugerId, HttpContext.Request.Method); //Logging the success
                return NoContent(); //Returning no content to indicate a successful update
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating fixed expense {Id}: {Message}.", id, ex.Message); //Logging the error if exception occurs
                return BadRequest(ex.Message); //Returning bad request with the error message
            }
        }

        [HttpDelete("{id}/slet")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string brugerId = GetUserId(); //Fetching user ID for security check
                _logger.LogInformation("Deleting fixed expense {Id} for user {BrugerId}. {Method}", id, brugerId, HttpContext.Request.Method); //Logging the action

                await _fudgifterService.DeleteFudgifter(brugerId, id); //Calling the service to delete the expense

                _logger.LogInformation("Successfully deleted fixed expense {Id} for user {BrugerId}. {Method}", id, brugerId, HttpContext.Request.Method); //Logging the success
                return NoContent(); //Returning no content to indicate a successful deletion
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting fixed expense {Id}: {Message}.", id, ex.Message); //Logging the error if exception occurs
                return BadRequest(ex.Message); //Returning bad request with the error message
            }
        }
    }
}
