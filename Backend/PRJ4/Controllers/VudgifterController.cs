using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using PRJ4.Data;
using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;
using PRJ4.Services;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace PRJ4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VudgifterController : ControllerBase
    {
        private readonly IVudgifterService _VudgifterService; //Service reference for business logic related to Vudgifter
        private readonly ILogger<VudgifterController> _logger; //Logging reference for the controller

        public VudgifterController(IVudgifterService VudgifterService, ILogger<VudgifterController> logger)//Inheritance from baseController, also name of controller, which is used in the route part, but Controller is removed from the name thx to ASP.NET
        {
            _VudgifterService = VudgifterService;
            _logger = logger;
        }

        private string GetUserId() //Function to retrieve the User ID from the JWT token
        {
            var claims = Request.HttpContext.User.Claims; //Getting HttpContext to extract user claims
            var userIdClaim = claims.FirstOrDefault(c => c.Type.Split('/').Last() == "nameidentifier");
            if(userIdClaim.Value != null)
            {
                return userIdClaim.Value; //Returning user ID if found in claims
            }
            return null; //Returning null if no user ID is found
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VudgifterResponseDTO>>> GetAllByUser() //Gets all variable expenses for the user
        {
            string brugerId = GetUserId(); //Security check by getting the user ID
            try
            {
                _logger.LogInformation("Fetching all variable expenses for user with ID: {BrugerId} {Method}", brugerId, HttpContext.Request.Method); //Logging the action

                var response = await _VudgifterService.GetAllByUser(brugerId); //Calling the service to fetch all variable expenses for the user
                return Ok(response); //Returning the fetched expenses
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching variable expenses for user"); //Logging the error if any
                return StatusCode(500, "An error occurred while processing your request."); //Returning a 500 status code if an error occurs
            }
        }

        [HttpPost]
        public async Task<ActionResult<VudgifterResponseDTO>> AddVudgifter(nyVudgifterDTO dto) //Adds a new variable expense for the user
        {
            try
            {
                string brugerId = GetUserId(); //Getting the user ID for security check
                _logger.LogInformation("Posting variable expense for user with ID {BrugerId}. {Method}", brugerId, HttpContext.Request.Method); //Logging the action

                var response = await _VudgifterService.AddVudgifter(brugerId, dto); //Calling the service to add the new variable expense
                return CreatedAtAction(nameof(GetAllByUser), new { id = response.VudgiftId }, response); //Returning the created expense
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while adding variable expense for user {ex}", ex.Message); //Logging the error if any
                return StatusCode(500, "An error occurred while processing your request."); //Returning a 500 status code if an error occurs
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVudgifter(int id, VudgifterUpdateDTO updateDTO) //Updates a specific variable expense
        {
            try
            {
                string brugerId = GetUserId(); //Fetching the user ID for security check
                _logger.LogInformation("Updating variable expense for user with ID {BrugerId}. {Method}", brugerId, HttpContext.Request.Method); //Logging the action

                //If the updateDTO contains no valid data, return a BadRequest response
                if (updateDTO == null || 
                    (!updateDTO.Pris.HasValue && string.IsNullOrWhiteSpace(updateDTO.Tekst) && !updateDTO.Dato.HasValue && !updateDTO.KategoriId.HasValue && string.IsNullOrWhiteSpace(updateDTO.KategoriNavn)))
                {
                    _logger.LogWarning("Update request for variable expense ID {VudgiftId} is empty or invalid.", id); //Logging the warning
                    return BadRequest("No valid data provided for update."); //Returning a bad request response
                }

                await _VudgifterService.UpdateVudgifter(brugerId, id, updateDTO); //Calling the service to update the expense
                return NoContent(); //Returning a no content response indicating a successful update
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid data provided for variable expense update."); //Logging the warning for invalid data
                return BadRequest(ex.Message); //Returning bad request with the exception message
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Variable expense not found for update."); //Logging the warning if the expense was not found
                return NotFound(ex.Message); //Returning a not found response if the expense doesn't exist
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating variable expense"); //Logging the error if any
                return StatusCode(500, "An error occurred while processing your request."); //Returning a 500 status code if an error occurs
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVudgifter(int id) //Deletes a variable expense for the user
        {
            try
            {
                string brugerId = GetUserId(); //Getting the user ID for security check
                _logger.LogInformation("Trying to delete variable expense {id} for UserId {brugerId}. {Method}", id, brugerId, HttpContext.Request.Method); //Logging the action

                await _VudgifterService.DeleteVudgifter(brugerId, id); //Calling the service to delete the expense
                return NoContent(); //Returning no content indicating a successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Variable expense not found for deletion."); //Logging the warning if the expense was not found
                return NotFound(ex.Message); //Returning a not found response if the expense doesn't exist
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting variable expense"); //Logging the error if any
                return StatusCode(500, "An error occurred while processing your request."); //Returning a 500 status code if an error occurs
            }
        }
    }
}
