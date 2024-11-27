using Azure;
using DnsClient.Protocol;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenAI;
using OpenAI.Chat;
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

        public async Task<string> GetCategorySuggestion(string description)
        {
            var prompt = $"Suggest a category with a respone of 1 word, for the following expense or income: '{description}'.";

            // Create a request for chat completion
            var chatRequest = _client.CompleteChat("{promt}",prompt);
            var repsonsechat = chatRequest.Value.Content[0].Text;
            return repsonsechat;
        }
    }
}
