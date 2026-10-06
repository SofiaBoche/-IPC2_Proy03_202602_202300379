using System.Text.RegularExpressions;

namespace ApiBackend.Utils
{
    public static class NitValidator
    {
        public static bool EsNitValido(string nit)
        {
            if (string.IsNullOrWhiteSpace(nit)) return false;

            nit = nit.Trim().ToUpper();

            if (!Regex.IsMatch(nit, @"^[0-9]{1,20}[0-9K]$"))
            {
                return false;
            }

            char digitoVerificadorEsperado = nit[nit.Length - 1];
            string parteNumerica = nit.Substring(0, nit.Length - 1);

            int suma = 0;
            int posicion = 1;

            for (int i = parteNumerica.Length - 1; i >= 0; i--)
            {
                int digito = parteNumerica[i] - '0';
                suma += digito * (posicion + 1);
                posicion++;
            }

            int modulo = suma % 11;
            int resta = 11 - modulo;
            int resultadoFinal = resta % 11;

            char digitoCalculado = (resultadoFinal == 10) ? 'K' : resultadoFinal.ToString()[0];

            return digitoVerificadorEsperado == digitoCalculado;
        }
    }
}