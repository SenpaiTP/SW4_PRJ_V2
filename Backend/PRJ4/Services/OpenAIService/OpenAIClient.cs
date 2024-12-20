using Azure;
using DnsClient.Protocol;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenAI;
using OpenAI.Chat;
using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.Repositories;
using System.Threading.Tasks;   

namespace PRJ4.Services
{
    public class OpenAIClient
    {
        private readonly ChatClient _client;

        public OpenAIClient(string apiKey)
        {

            // Initialize the OpenAI client with the provided API key
            ChatClient client = new(model: "gpt-4o", apiKey: apiKey);
            _client = client;
        }

        public async Task<string> GetCategorySuggestion(string description, IEnumerable<Kategori> currentCategories)
        {
            string categoriesList = string.Join(", ", currentCategories.Select(c => c.KategoriNavn));
            var prompt = $"Foreslå en kategori med 1 ord svar, for følgende udgift eller indtægt: '{description}', hvis en eksisterende kategori allerede eksisterer fra denne liste over kategorier, skal du bruge den eller oprette en ny, der passer bedre {categoriesList}. Svar på dansk.";

            

            // Create a request for chat completion
            var chatRequest = _client.CompleteChat("{promt}", prompt);
            var repsonsechat = chatRequest.Value.Content[0].Text;
            if (repsonsechat == null)
            {
                return null;
            }
            return repsonsechat;
        }
    }
}
