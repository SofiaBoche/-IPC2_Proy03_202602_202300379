using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiBackend.Models
{
    public class RespuestaRtuDto
    {
        [JsonPropertyName("CONTRIBUYENTES NUEVOS")]
        public int ContribuyentesNuevos { get; set; }

        [JsonPropertyName("CONTRIBUYENTES ACTUALIZADOS")]
        public int ContribuyentesActualizados { get; set; }

        [JsonPropertyName("NITS INVALIDOS")]
        public int NitsInvalidos { get; set; }
    }
    public class DetalleAprobacionDto
    {
        [JsonPropertyName("referencia")]
        public string Referencia { get; set; } = string.Empty;

        [JsonPropertyName("codigoAprobacion")]
        public string CodigoAprobacion { get; set; } = string.Empty;
    }

    public class ResultadoAutorizacionesDto
    {
        [JsonPropertyName("cantidadAutorizacionesAprobadas")]
        public int CantidadAutorizacionesAprobadas { get; set; }

        [JsonPropertyName("cantidadAutorizacionesRechazadasPorNitEmisorInexistente")]
        public int CantidadAutorizacionesRechazadasPorNitEmisorInexistente { get; set; }

        [JsonPropertyName("cantidadAutorizacionesRechazadasPorNitReceptorInexistente")]
        public int CantidadAutorizacionesRechazadasPorNitReceptorInexistente { get; set; }

        [JsonPropertyName("detalleAprobaciones")]
        public List<DetalleAprobacionDto> DetalleAprobaciones { get; set; } = new List<DetalleAprobacionDto>();
    }

    public class RespuestaAutorizacionesWrapper
    {
        [JsonPropertyName("resultadoAutorizaciones")]
        public ResultadoAutorizacionesDto ResultadoAutorizaciones { get; set; } = new ResultadoAutorizacionesDto();
    }
}