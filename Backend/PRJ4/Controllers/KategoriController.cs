using Microsoft.AspNetCore.Mvc;
using PRJ4.DTOs;
using PRJ4.Repositories;
using PRJ4.Servies;

namespace PRJ4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KategoriController : ControllerBase
    {
        private readonly IKategori _kategorirepo;
        //private readonly OpenAIClient _openAIClient;
        private readonly CategorizerSuggester _suggester;

        public KategoriController(IKategori kategorirepo, CategorizerSuggester suggester )//OpenAIClient openAIClient)
        {
            _kategorirepo = kategorirepo;
            //_openAIClient = openAIClient;
            _suggester = suggester;
        }

        // POST: api/Kategori/Suggest
        [HttpPost("suggest")]
        public async Task<IActionResult> SuggestCategory([FromBody] KategoriSuggestionDTO descriptionInput)
        {
            if (string.IsNullOrWhiteSpace(descriptionInput.Description))
            {
                return BadRequest("Description cannot be empty.");
            }
            //string suggestedCategory = descriptionInput.Description;
            // Step 1: Get category suggestion from OpenAI
            var suggestedCategory = _suggester.PredictCategory(descriptionInput.Description);
            Console.WriteLine(suggestedCategory);

            // Step 2: Find the best matching category using fuzzy matching
            suggestedCategory = suggestedCategory.ToLower();
            var bestMatchCategory = await _kategorirepo.SearchByBestFuzzyMatch(suggestedCategory);

            if (bestMatchCategory == null)
            {
                // If a similar category is found, return the existing category
                return Ok(suggestedCategory);
            }
            else
            {
                suggestedCategory = bestMatchCategory.Navn;
            }
            // Step 3: If no similar category is found, return the suggested category as a new one
            return Ok(suggestedCategory);
        }
    }
}
