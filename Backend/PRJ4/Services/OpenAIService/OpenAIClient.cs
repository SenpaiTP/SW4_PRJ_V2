using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using OpenAI.Chat;
using System.Threading.Tasks;

namespace PRJ4.Services
{
    public class OpenAIClient
    {
        private readonly ChatClient client;

        public OpenAIClient(string apiKey)
        {
            // Initialize the OpenAI API client with the provided API key
            client = new ChatClient("gpt-3.5-turbo",apiKey);
        }

        public async Task<string> GetCategorySuggestion(string description)
        {
            var prompt = $"Suggest a category for the following expense or income: '{description}'.";
           ChatCompletion chatCompletion = client.CompleteChat(prompt);
            if (chatCompletion == null){
                return "null";
            }
            // Return the suggestion text from the first completion result
            return chatCompletion.Content.ToString();
        }
    }
}
