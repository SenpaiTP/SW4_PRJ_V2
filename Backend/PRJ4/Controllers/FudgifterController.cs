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
//using PRJ4.Services;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
namespace PRJ4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class FudgifterController:ControllerBase
    {
        private readonly IFudgifter _fudgifterRepo;
        //private readonly IBrugerRepo _brugerRepo;
        private readonly IKategori _kategoriRepo;

        public FudgifterController(IFudgifter fudgifterRepo, IKategori kategoriRepo /*IBrugerRepo brugerRepo*/)
        {
           _fudgifterRepo = fudgifterRepo;
           _kategoriRepo = kategoriRepo;
            /*_brugerRepo = brugerRepo;*/
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FudgifterResponseDTO>>> GetAllByUser()
        {
            // Get the BrugerId (User ID) from the JWT token's "sub" claim
            // var brugerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            // if (string.IsNullOrEmpty(brugerIdClaim) || !int.TryParse(brugerIdClaim, out int brugerId))
            // {
            //     return Unauthorized("User ID claim missing or invalid.");
            // }

            // Retrieve the user's Fudgifter records using the BrugerId
            var fudgifter = await _fudgifterRepo.GetAllByUserId(1); // replace 1 with BrugerId

            // Map to response DTO
            var responseDTOs = fudgifter.Select(f => new FudgifterResponseDTO
            {
                FudgiftId = f.FudgiftId,
                Pris = f.Pris,
                Tekst = f.Tekst,
                KategoriName = f.Kategori?.Name,
                Dato = f.Dato
            });

            return Ok(responseDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<Fudgifter>> Add(newFudgifterDTO fudgifter)
        {
            // Validate that the input data is not null
            if (fudgifter == null)
            {
                return BadRequest("Fudgifter data cannot be null");
            }

            // Extract BrugerId from the JWT token (the "sub" claim or NameIdentifier)
            // var brugerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            // if (string.IsNullOrEmpty(brugerIdClaim) || !int.TryParse(brugerIdClaim, out int authenticatedBrugerId))
            // {
            //     return Unauthorized("Invalid token or missing BrugerId.");
            // }

            // Check if the user (Bruger) exists
            // Bruger bruger = await _brugerRepo.GetByIdAsync(authenticatedBrugerId);
            // if (bruger == null)
            // {
            //     return NotFound($"Bruger with ID {authenticatedBrugerId} not found.");
            // }

            Kategori kategori;
            // Ensure Kategori exists or create a new one if KategoriId is not provide
            if (fudgifter.KategoriId == 0)
            {
                kategori = await _kategoriRepo.NewKategori(fudgifter.KategoriName);
            }
            else
            {
                kategori = await _kategoriRepo.GetByIdAsync(fudgifter.KategoriId);
                if (kategori == null)
                {
                    return NotFound($"Kategori with ID {fudgifter.KategoriId} not found.");
                }
            }

            // Map DTO to Fudgifter model
            Fudgifter newFudgifter = new Fudgifter
            {
                Pris = fudgifter.Pris,
                Tekst = fudgifter.Tekst,
                Dato = fudgifter.Dato,
                KategoriId = kategori.KategoriId, // Set the actual Kategori ID
                BrugerId = 1,       // Use authenticated BrugerId
                Kategori = kategori,              // Link the Kategori entity
                //Bruger = bruger                   // Link the Bruger entity
            };

            // Save the new Fudgifter to the database
            await _fudgifterRepo.AddAsync(newFudgifter);
            await _fudgifterRepo.SaveChangesAsync();

            // Return the newly created Fudgifter object
            return CreatedAtAction(nameof(Add), new { id = newFudgifter.FudgiftId }, newFudgifter);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<Fudgifter>> Updatefudgifter(int id, [FromBody] FudgifterUpdateDTO updateDTO)
        {
            // var brugerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            // if (string.IsNullOrEmpty(brugerIdClaim) || !int.TryParse(brugerIdClaim, out int brugerId))
            // {
            //     return Unauthorized("User ID claim missing or invalid.");
            // }

            // Step 1: Get the fudgifter entity from the database
            var fudgifter = await _fudgifterRepo.GetByIdAsync(id);
            if (fudgifter == null)
            {
                return NotFound("fudgifter not found.");
            }

            // Ensure that the Vudgift belongs to the authenticated user
            // if (fudgifter.BrugerId != brugerId)
            // {
            //     return Unauthorized("You do not have permission to update this Vudgift.");
            // }

            // Step 2: Handle dynamic updates for each property only if it's provided in the DTO
            if (updateDTO.Pris.HasValue)
            {
                fudgifter.Pris = updateDTO.Pris.Value;
            }

            if (!string.IsNullOrEmpty(updateDTO.Tekst))
            {
                fudgifter.Tekst = updateDTO.Tekst;
            }

            if (updateDTO.Dato.HasValue)
            {
                fudgifter.Dato = updateDTO.Dato.Value;
            }

            // Step 3: Handle the Kategori update
           if (updateDTO.KategoriId.HasValue)
            {
                var kategori = await _kategoriRepo.GetByIdAsync(updateDTO.KategoriId.Value);
                if (kategori == null)
                {
                    return BadRequest("No kategori exist by that id.");
                }

                fudgifter.KategoriId = kategori.KategoriId;
                fudgifter.Kategori = kategori;
            }
            else if(!string.IsNullOrWhiteSpace(updateDTO.KategoriName))
            {
                var kategori = await _kategoriRepo.NewKategori(updateDTO.KategoriName);
                if (kategori == null)
                {
                    return BadRequest("New Kategori wasnt created.");
                }

                fudgifter.KategoriId = kategori.KategoriId;
                fudgifter.Kategori = kategori;
            }

            // Step 4: Save the updated entity
            _fudgifterRepo.Update(fudgifter);
            await _fudgifterRepo.SaveChangesAsync();

            // Step 5: Return the updated entity
            return Ok(fudgifter);
        }

        [HttpDelete("{id}/delete")]
        public async Task<ActionResult<Fudgifter>> DeleteFudgiftById(int id)
        {
            Console.WriteLine("Made it here 1");
            if (id <= 0)
            {
                return NotFound("Fudigft id cannot be less or equal to 0.");
            }
            Fudgifter fudgifter = await _fudgifterRepo.GetByIdAsync(id);
            Console.WriteLine("Made it here 2");
            if(fudgifter == null){ return BadRequest($"Couldnt find fast udgift with id: {id}");}
            Console.WriteLine("Made it here 3");
            _fudgifterRepo.Delete(id);
            await _fudgifterRepo.SaveChangesAsync();
            return NoContent();
        }

    }
}