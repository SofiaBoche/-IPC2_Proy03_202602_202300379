using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebFrontend.Services;
using System.Threading.Tasks;

namespace WebFrontend.Pages
{
    public class CargarDatosModel : PageModel
    {
        private readonly ApiService _apiService;

        [BindProperty]
        public string JsonRtu { get; set; } = string.Empty;

        [BindProperty]
        public string JsonFacturas { get; set; } = string.Empty;

        public string ResultadoJson { get; set; } = string.Empty;

        public CargarDatosModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostProcesarRtuAsync()
        {
            if (!string.IsNullOrWhiteSpace(JsonRtu))
            {
                ResultadoJson = await _apiService.EnviarRtuAsync(JsonRtu);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostProcesarFacturasAsync()
        {
            if (!string.IsNullOrWhiteSpace(JsonFacturas))
            {
                ResultadoJson = await _apiService.EnviarFacturasAsync(JsonFacturas);
            }
            return Page();
        }
    }
}