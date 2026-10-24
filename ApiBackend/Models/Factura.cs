using System.Text.Json.Serialization;

namespace ApiBackend.Models
{
    public class SolicitudFacturaInputDto
    {
        [JsonPropertyName("TIEMPO")]
        public string Tiempo { get; set; } = string.Empty;

        [JsonPropertyName("REFERENCIA")]
        public string Referencia { get; set; } = string.Empty;

        [JsonPropertyName("NIT EMISOR")]
        public string NitEmisor { get; set; } = string.Empty;

        [JsonPropertyName("NIT RECEPTOR")]
        public string NitReceptor { get; set; } = string.Empty;

        [JsonPropertyName("VALOR")]
        public decimal Valor { get; set; }

        [JsonPropertyName("IVA")]
        public decimal Iva { get; set; }

        [JsonPropertyName("TOTAL")]
        public decimal Total { get; set; }
    }
    public class FacturaDto
    {
        public string Referencia { get; set; } = string.Empty;
        public string NitEmisor { get; set; } = string.Empty;
        public string NitReceptor { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public bool EsValida { get; set; }
        public string? CodigoAprobacion { get; set; }
    }
}