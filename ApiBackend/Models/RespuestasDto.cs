using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiBackend.Models
{
    public class RespuestaRtuDto
    {
        [JsonPropertyName("CONTRIBUYENTES_NUEVOS")]
        public int ContribuyentesNuevos { get; set; }

        [JsonPropertyName("CONTRIBUYENTES_ACTUALIZADOS")]
        public int ContribuyentesActualizados { get; set; }

        [JsonPropertyName("NITS_INVALIDOS")]
        public int NitsInvalidos { get; set; }
    }

    public class DetalleAprobacionDto
    {
        [JsonPropertyName("referencia")]
        public string Referencia { get; set; } = string.Empty;

        [JsonPropertyName("codigoAprobacion")]
        public string CodigoAprobacion { get; set; } = string.Empty;
    }

    public class ErroresFacturaDto
    {
        [JsonPropertyName("nitEmisorInvalido")]
        public int NitEmisorInvalido { get; set; }

        [JsonPropertyName("nitReceptorInvalido")]
        public int NitReceptorInvalido { get; set; }

        [JsonPropertyName("nitEmisorInexistente")]
        public int NitEmisorInexistente { get; set; }

        [JsonPropertyName("nitReceptorInexistente")]
        public int NitReceptorInexistente { get; set; }

        [JsonPropertyName("ivaMalCalculado")]
        public int IvaMalCalculado { get; set; }

        [JsonPropertyName("totalMalCalculado")]
        public int TotalMalCalculado { get; set; }

        [JsonPropertyName("referenciaDuplicada")]
        public int ReferenciaDuplicada { get; set; }
    }

    public class ResultadoAutorizacionesDto
    {
        [JsonPropertyName("aprobadas")]
        public int Aprobadas { get; set; }

        [JsonPropertyName("rechazadas")]
        public int Rechazadas { get; set; }

        [JsonPropertyName("errores")]
        public ErroresFacturaDto Errores { get; set; } = new ErroresFacturaDto();

        [JsonPropertyName("descartadasPorFormato")]
        public int DescartadasPorFormato { get; set; }

        [JsonPropertyName("detalleAprobaciones")]
        public List<DetalleAprobacionDto> DetalleAprobaciones { get; set; } = new List<DetalleAprobacionDto>();
    }

    public class RespuestaAutorizacionesWrapper
    {
        [JsonPropertyName("resultadoAutorizaciones")]
        public ResultadoAutorizacionesDto ResultadoAutorizaciones { get; set; } = new ResultadoAutorizacionesDto();
    }
}