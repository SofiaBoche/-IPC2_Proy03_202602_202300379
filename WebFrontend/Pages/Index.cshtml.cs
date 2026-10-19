using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebFrontend.Services;

namespace WebFrontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;

        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostResetearAsync()
        {
            bool exito = await _apiService.ResetearSistemaAsync();
            if (exito)
            {
                TempData["Mensaje"] = "El sistema ha sido reiniciado correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "No se pudo conectar con la API para reiniciar el sistema.";
            }

            return RedirectToPage();
        }
    }
}
