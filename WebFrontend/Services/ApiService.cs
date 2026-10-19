using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebFrontend.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5234/api/"; 
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ResetearSistemaAsync()
        {
            var response = await _httpClient.PostAsync($"{BaseUrl}resetear", null);
            return response.IsSuccessStatusCode;
        }
    }
}