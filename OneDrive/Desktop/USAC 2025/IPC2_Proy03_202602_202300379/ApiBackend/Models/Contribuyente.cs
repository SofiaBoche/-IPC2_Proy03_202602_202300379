namespace ApiBackend.Models
{
    public class Contribuyente
    {
        public string Nit { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;

        public Contribuyente() { }

        public Contribuyente(string nit, string nombre)
        {
            Nit = nit;
            Nombre = nombre;
        }
    }
}