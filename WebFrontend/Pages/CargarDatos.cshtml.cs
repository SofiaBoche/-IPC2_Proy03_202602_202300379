using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebFrontend.Pages
{
    public class CargarDatosModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "http://localhost:5234/api/"; 

        public string? ResultadoJson { get; set; }

        public CargarDatosModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostProcesarRtuAsync(string jsonRtu)
        {
            if (string.IsNullOrWhiteSpace(jsonRtu)) return Page();

            var content = new StringContent(jsonRtu, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ApiBaseUrl}procesar-rtu", content);

            if (response.IsSuccessStatusCode)
            {
                ResultadoJson = await response.Content.ReadAsStringAsync();
            }
            else
            {
                ResultadoJson = "Error al comunicarse con el API de Backend.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostProcesarFacturasAsync(string jsonFacturas)
        {
            if (string.IsNullOrWhiteSpace(jsonFacturas)) return Page();

            var content = new StringContent(jsonFacturas, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{ApiBaseUrl}procesar-facturas", content);

            if (response.IsSuccessStatusCode)
            {
                ResultadoJson = await response.Content.ReadAsStringAsync();
            }
            else
            {
                ResultadoJson = "Error al comunicarse con el API de Backend.";
            }

            return Page();
        }
    }
}