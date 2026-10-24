using Microsoft.AspNetCore.Mvc;
using ApiBackend.Models;
using ApiBackend.Services;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiBackend.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly DataService _dataService;

        public ApiController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("cargardatos")]
        public async Task<IActionResult> CargarDatos()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                string rawJson = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(rawJson))
                {
                    return BadRequest(new { error = "El cuerpo de la solicitud está vacío." });
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var listaRtu = JsonSerializer.Deserialize<List<ContribuyenteInputDto>>(rawJson, options);
                var respuesta = _dataService.ProcesarRtu(listaRtu ?? new List<ContribuyenteInputDto>());
                
                return Ok(respuesta);
            }
            catch
            {
                return BadRequest(new { error = "El formato JSON de RTU es inválido." });
            }
        }

        [HttpPost("autorizaciones")]
        public async Task<IActionResult> AutorizarFacturas()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                string rawJson = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(rawJson))
                {
                    return BadRequest(new { error = "El cuerpo de la solicitud está vacío." });
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var listaFacturas = JsonSerializer.Deserialize<List<SolicitudFacturaInputDto>>(rawJson, options);
                var respuesta = _dataService.ProcesarFacturas(listaFacturas ?? new List<SolicitudFacturaInputDto>());
                
                return Ok(respuesta);
            }
            catch
            {
                return BadRequest(new { error = "El formato JSON de Facturas es inválido." });
            }
        }

        [HttpPost("resetear")]
        public IActionResult Resetear()
        {
            _dataService.ResetearSistema();
            return Ok(new { mensaje = "Sistema reiniciado con éxito." });
        }
        [HttpGet("contribuyentes")]
        public IActionResult ObtenerContribuyentes()
        {
            var lista = _dataService.ObtenerTodosContribuyentes();
            return Ok(lista);
        }

        [HttpGet("historial")]
        public IActionResult ObtenerHistorial()
        {
            var resumen = _dataService.ObtenerResumenHistorial();
            return Ok(resumen);
        }
        
        [HttpGet("consulta-datos")]
        public IActionResult ObtenerConsultaDatos()
        {
            var facturas = _dataService.ObtenerFacturasProcesadas();
            return Ok(facturas);
        }
    }
}