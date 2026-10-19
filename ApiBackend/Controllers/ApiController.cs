using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ApiBackend.Models;
using ApiBackend.Services;

namespace ApiBackend.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly DataService _dataService = new DataService();

        [HttpPost("procesar-rtu")]
        public IActionResult ProcesarRtu([FromBody] List<Contribuyente> contribuyentes)
        {
            var resultado = _dataService.ProcesarRtu(contribuyentes);
            return Ok(resultado);
        }

        [HttpPost("procesar-facturas")]
        public IActionResult ProcesarFacturas([FromBody] List<Factura> facturas)
        {
            var resultado = _dataService.ProcesarFacturas(facturas);
            return Ok(new RespuestaAutorizacionesWrapper { ResultadoAutorizaciones = resultado });
        }

        [HttpGet("consulta-datos")]
        public IActionResult ObtenerFacturas()
        {
            var facturas = _dataService.ObtenerFacturas();
            return Ok(facturas);
        }

        [HttpPost("resetear")]
        public IActionResult Resetear()
        {
            _dataService.ResetearSistema();
            return Ok(new { mensaje = "Sistema reiniciado con éxito." });
        }
    }
}