using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebFrontend.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ResetearSistemaAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("http://localhost:5234/api/resetear", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> EnviarRtuAsync(string jsonContent)
        {
            return await PostJsonAsync("http://localhost:5234/api/cargardatos", jsonContent);
        }

        public async Task<string> EnviarFacturasAsync(string jsonContent)
        {
            return await PostJsonAsync("http://localhost:5234/api/autorizaciones", jsonContent);
        }

        private async Task<string> PostJsonAsync(string url, string jsonContent)
        {
            try
            {
                var content = new StringContent(jsonContent ?? "", Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                
                string responseBody = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    return responseBody;
                }

                return $"{{\"error\": \"El servidor respondió con código {response.StatusCode}\", \"respuesta\": \"{responseBody.Replace("\"", "\\\"")}\"}}";
            }
            catch (Exception ex)
            {
                return $"{{\"error\": \"Error de conexión con el Backend\", \"mensaje\": \"{ex.Message}\"}}";
            }
        }

        // ==========================================
        // MÉTODOS AGREGADOS PARA LA PÁGINA DE REPORTES
        // ==========================================

        public async Task<string> ObtenerContribuyentesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5234/api/contribuyentes");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return "[]";
            }
            catch
            {
                return "[]";
            }
        }

        public async Task<string> ObtenerResumenReportesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5234/api/historial");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return "{}";
            }
            catch
            {
                return "{}";
            }
        }
    }
}