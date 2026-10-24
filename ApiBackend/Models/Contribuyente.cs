using System.Text.Json.Serialization;

namespace ApiBackend.Models
{
    public class Contribuyente
    {
        public string Nit { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }

    public class ContribuyenteInputDto
    {
        [JsonPropertyName("NIT")]
        public string Nit { get; set; } = string.Empty;

        [JsonPropertyName("NOMBRE")]
        public string Nombre { get; set; } = string.Empty;
    }
}