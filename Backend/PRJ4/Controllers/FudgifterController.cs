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
    public class FudgifterController:ControllerBase
    {
        private readonly IFudgifter _fudgifterRepo;
        private readonly IBrugerRepo _brugerRepo;
        private readonly IKategori _kategoriRepo;

        public FudgifterController(IFudgifter fudgifterRepo, IKategori kategoriRepo,IBrugerRepo brugerRepo)
        {
           _fudgifterRepo = fudgifterRepo;
           _kategoriRepo = kategoriRepo;
            _brugerRepo = brugerRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FudgifterResponseDTO>>> GetAllByUser()
        {
            // Get the BrugerId (User ID) from the JWT token's "sub" claim
            var brugerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(brugerIdClaim) || !int.TryParse(brugerIdClaim, out int brugerIdClaimed))
            {
                return Unauthorized("User ID claim manglende eller invalid.");
            }

            // Retrieve the user's Fudgifter records using the BrugerId
            var fudgifter = await _fudgifterRepo.GetAllByUserId(brugerIdClaimed); // replace 1 with BrugerId

            // Map to response DTO
            var responseDTOs = fudgifter.Select(f => new FudgifterResponseDTO
            {
                FudgiftId = f.FudgiftId,
                Pris = f.Pris,
                Tekst = f.Tekst,
                KategoriNavn = f.Kategori?.Navn,
                Dato = f.Dato
            });

            return Ok(responseDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<FudgifterResponseDTO>> Add(nyFudgifterDTO fudgifter)
        {
            // Validate that the input data is not null
            if (fudgifter == null)
            {
                return BadRequest("Fudgifter data kan ikke være null");
            }

            // Extract BrugerId from the JWT token (the "sub" claim or NameIdentifier)
            var brugerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(brugerIdClaim) || !int.TryParse(brugerIdClaim, out int brugerIdClaimed))
            {
                return Unauthorized("Invalid token or missing BrugerId.");
            }

            // Check if the user (Bruger) exists
            Bruger bruger = await _brugerRepo.GetByIdAsync(brugerIdClaimed);
            if (bruger == null)
            {
                return NotFound($"Bruger with ID {brugerIdClaimed} not found.");
            }

            Kategori kategori;
            // Ensure Kategori exists or create a new one if KategoriId is not provided
            if (fudgifter.KategoriId == 0)
            {
                kategori = await _kategoriRepo.NyKategori(fudgifter.KategoriNavn);
            }
            else
            {
                kategori = await _kategoriRepo.GetByIdAsync(fudgifter.KategoriId);
                if (kategori == null)
                {
                    return NotFound($"Kategori med Id {fudgifter.KategoriId} ikke fundet.");
                }
            }

            // Map DTO to Fudgifter model
            Fudgifter nyFudgifter = new Fudgifter
            {
                Pris = fudgifter.Pris,
                Tekst = fudgifter.Tekst,
                Dato = fudgifter.Dato,
                KategoriId = kategori.KategoriId,
                BrugerId = brugerIdClaimed,
                Kategori = kategori,
                Bruger = bruger
            };

            // Save the new Fudgifter to the database
            await _fudgifterRepo.AddAsync(nyFudgifter);
            await _fudgifterRepo.SaveChangesAsync();

            // Map to FudgifterResponseDTO
            var responseDto = new FudgifterResponseDTO
            {
                FudgiftId = nyFudgifter.FudgiftId,
                Pris = nyFudgifter.Pris,
                Tekst = nyFudgifter.Tekst,
                Dato = nyFudgifter.Dato,
                KategoriNavn = kategori.Navn // Assuming Kategori has a property "Navn"
            };

            // Return the newly created DTO object
            return CreatedAtAction(nameof(Add), new { id = responseDto.FudgiftId }, responseDto);
        }


        [HttpPut("opdater/{id}")]
        public async Task<ActionResult<Fudgifter>> Opdaterfudgifter(int id, [FromBody] FudgifterUpdateDTO updateDTO)
        {
            var brugerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(brugerIdClaim) || !int.TryParse(brugerIdClaim, out int brugerId))
            {
                return Unauthorized("Bruger Id claim manglende eller invalid.");
            }

            // Step 1: Get the fudgifter entity from the database
            var fudgifter = await _fudgifterRepo.GetByIdAsync(id);
            if (fudgifter == null)
            {
                return NotFound("fudgifter ikke fundet.");
            }

            //Ensure that the Vudgift belongs to the authenticated user
            if (fudgifter.BrugerId != brugerId)
            {
                return Unauthorized("Du har ikke tilladelse til at ændre Vudgift.");
            }

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
                    return BadRequest("Ingen kategori eksistere på denne id.");
                }

                fudgifter.KategoriId = kategori.KategoriId;
                fudgifter.Kategori = kategori;
            }
            else if(!string.IsNullOrWhiteSpace(updateDTO.KategoriNavn))
            {
                var kategori = await _kategoriRepo.NyKategori(updateDTO.KategoriNavn);
                if (kategori == null)
                {
                    return BadRequest("Ny Kategori er ikke skabt.");
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

        [HttpDelete("{id}/slet")]
        public async Task<ActionResult<Fudgifter>> SletFudgiftVedId(int id)
        {
            var brugerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(brugerIdClaim) || !int.TryParse(brugerIdClaim, out int brugerIdClaimed))
            {
                return Unauthorized("Bruger Id claim manglende eller invalid.");
            }
            if (id <= 0)
            {
                return NotFound("Fudigft id kan ikke være mindre eller ligmed 0.");
            }
            Fudgifter fudgifter = await _fudgifterRepo.GetByIdAsync(id);
          
            if(fudgifter == null){ return BadRequest($"Kan ikke finde fast udgift med id: {id}");}
            
            _fudgifterRepo.Delete(id);
            await _fudgifterRepo.SaveChangesAsync();
            return NoContent();
        }

    }
}