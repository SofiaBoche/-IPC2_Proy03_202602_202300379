using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebFrontend.Services;
using System.Threading.Tasks;

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
            bool resultado = await _apiService.ResetearSistemaAsync();
            if (resultado)
            {
                return new JsonResult(new { success = true, message = "Sistema reiniciado con éxito." });
            }

            return new JsonResult(new { success = false, message = "Ocurrió un error al intentar reiniciar el sistema." });
        }
    }
}