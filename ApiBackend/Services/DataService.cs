using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using ApiBackend.Models;

namespace ApiBackend.Services
{
    public class DataService
    {
        private static List<Contribuyente> _contribuyentes = new List<Contribuyente>();
        private static List<string> _referenciasProcesadas = new List<string>();
        private static List<FacturaDto> _historialFacturas = new List<FacturaDto>();
        private static readonly string ArchivoRtu = "rtu_data.json";

        public DataService()
        {
            CargarRtuDesdeArchivo();
        }

        public void ResetearSistema()
        {
            _contribuyentes.Clear();
            _referenciasProcesadas.Clear();
            _historialFacturas.Clear();
            if (File.Exists(ArchivoRtu))
            {
                try
                {
                    File.Delete(ArchivoRtu);
                }
                catch { }
            }
        }

        public bool EsNitValido(string? nit)
        {
            if (string.IsNullOrWhiteSpace(nit)) return false;

            string nitLimpio = NormalizarNit(nit);
            if (!Regex.IsMatch(nitLimpio, @"^[0-9]{1,20}[0-9K]$")) return false;

            char digitoVerificador = nitLimpio[^1];
            string cuerpo = nitLimpio[..^1];

            int suma = 0;
            int peso = 2;

            for (int i = cuerpo.Length - 1; i >= 0; i--)
            {
                suma += (cuerpo[i] - '0') * peso;
                peso++;
            }

            int modulo = suma % 11;
            int resta = 11 - modulo;
            int resultadoFinal = resta % 11;

            char digitoCalculado = (resultadoFinal == 10) ? 'K' : resultadoFinal.ToString()[0];

            return digitoVerificador == digitoCalculado;
        }

        private string NormalizarNit(string nit)
        {
            return nit.Trim().Replace("-", "").ToUpper();
        }

        public RespuestaRtuDto ProcesarRtu(List<ContribuyenteInputDto> listaRtu)
        {
            int nuevos = 0;
            int actualizados = 0;
            int invalidos = 0;

            if (listaRtu == null || !listaRtu.Any())
            {
                return new RespuestaRtuDto();
            }

            foreach (var item in listaRtu)
            {
                if (string.IsNullOrWhiteSpace(item.Nit) || !EsNitValido(item.Nit))
                {
                    invalidos++;
                    continue;
                }

                string nitNormal = NormalizarNit(item.Nit);
                var existe = _contribuyentes.FirstOrDefault(c => NormalizarNit(c.Nit) == nitNormal);

                if (existe != null)
                {
                    existe.Nombre = item.Nombre ?? existe.Nombre;
                    actualizados++;
                }
                else
                {
                    _contribuyentes.Add(new Contribuyente
                    {
                        Nit = nitNormal,
                        Nombre = item.Nombre ?? ""
                    });
                    nuevos++;
                }
            }

            GuardarRtuEnArchivo();

            return new RespuestaRtuDto
            {
                ContribuyentesNuevos = nuevos,
                ContribuyentesActualizados = actualizados,
                NitsInvalidos = invalidos
            };
        }

        public RespuestaAutorizacionesWrapper ProcesarFacturas(List<SolicitudFacturaInputDto> listaFacturas)
        {
            var resultado = new ResultadoAutorizacionesDto();
            var errores = new ErroresFacturaDto();

            if (listaFacturas == null)
            {
                return new RespuestaAutorizacionesWrapper { ResultadoAutorizaciones = resultado };
            }

            foreach (var f in listaFacturas)
            {
                // Validación de formato: si falta algún campo clave
                if (string.IsNullOrWhiteSpace(f.NitEmisor) || 
                    string.IsNullOrWhiteSpace(f.NitReceptor) || 
                    string.IsNullOrWhiteSpace(f.Referencia))
                {
                    resultado.DescartadasPorFormato++;

                    // Se registra en el historial para visualizar el rechazo por formato en reportes
                    _historialFacturas.Add(new FacturaDto
                    {
                        Referencia = string.IsNullOrWhiteSpace(f.Referencia) ? "SIN_REF" : f.Referencia,
                        NitEmisor = string.IsNullOrWhiteSpace(f.NitEmisor) ? "N/A" : f.NitEmisor,
                        NitReceptor = string.IsNullOrWhiteSpace(f.NitReceptor) ? "N/A" : f.NitReceptor,
                        Valor = f.Valor,
                        Iva = f.Iva,
                        Total = f.Total,
                        EsValida = false,
                        CodigoAprobacion = null
                    });

                    continue;
                }

                bool tieneError = false;

                if (!EsNitValido(f.NitEmisor))
                {
                    errores.NitEmisorInvalido++;
                    tieneError = true;
                }

                if (!EsNitValido(f.NitReceptor))
                {
                    errores.NitReceptorInvalido++;
                    tieneError = true;
                }

                string nitEmisorNorm = NormalizarNit(f.NitEmisor);
                string nitReceptorNorm = NormalizarNit(f.NitReceptor);

                if (EsNitValido(f.NitEmisor) && !_contribuyentes.Any(c => NormalizarNit(c.Nit) == nitEmisorNorm))
                {
                    errores.NitEmisorInexistente++;
                    tieneError = true;
                }

                if (EsNitValido(f.NitReceptor) && !_contribuyentes.Any(c => NormalizarNit(c.Nit) == nitReceptorNorm))
                {
                    errores.NitReceptorInexistente++;
                    tieneError = true;
                }

                decimal ivaCalculado = Math.Round(f.Valor * 0.12m, 2);
                if (Math.Abs(f.Iva - ivaCalculado) > 0.01m)
                {
                    errores.IvaMalCalculado++;
                    tieneError = true;
                }

                decimal totalCalculado = Math.Round(f.Valor + f.Iva, 2);
                if (Math.Abs(f.Total - totalCalculado) > 0.01m)
                {
                    errores.TotalMalCalculado++;
                    tieneError = true;
                }

                if (_referenciasProcesadas.Contains(f.Referencia.Trim()))
                {
                    errores.ReferenciaDuplicada++;
                    tieneError = true;
                }

                if (tieneError)
                {
                    resultado.Rechazadas++;

                    _historialFacturas.Add(new FacturaDto
                    {
                        Referencia = f.Referencia,
                        NitEmisor = f.NitEmisor,
                        NitReceptor = f.NitReceptor,
                        Valor = f.Valor,
                        Iva = f.Iva,
                        Total = f.Total,
                        EsValida = false,
                        CodigoAprobacion = null
                    });
                }
                else
                {
                    resultado.Aprobadas++;
                    string codigo = $"APROBADO-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
                    _referenciasProcesadas.Add(f.Referencia.Trim());

                    resultado.DetalleAprobaciones.Add(new DetalleAprobacionDto
                    {
                        Referencia = f.Referencia,
                        CodigoAprobacion = codigo
                    });

                    _historialFacturas.Add(new FacturaDto
                    {
                        Referencia = f.Referencia,
                        NitEmisor = f.NitEmisor,
                        NitReceptor = f.NitReceptor,
                        Valor = f.Valor,
                        Iva = f.Iva,
                        Total = f.Total,
                        EsValida = true,
                        CodigoAprobacion = codigo
                    });
                }
            }

            resultado.Errores = errores;
            return new RespuestaAutorizacionesWrapper { ResultadoAutorizaciones = resultado };
        }

        public List<Contribuyente> ObtenerTodosContribuyentes()
        {
            return _contribuyentes;
        }

        public object ObtenerResumenHistorial()
        {
            return new 
            {
                totalContribuyentes = _contribuyentes.Count,
                totalReferenciasProcesadas = _referenciasProcesadas.Count
            };
        }

        public List<FacturaDto> ObtenerFacturasProcesadas()
        {
            return _historialFacturas;
        }

        private void GuardarRtuEnArchivo()
        {
            try
            {
                string json = JsonSerializer.Serialize(_contribuyentes);
                File.WriteAllText(ArchivoRtu, json);
            }
            catch { }
        }

        private void CargarRtuDesdeArchivo()
        {
            try
            {
                if (File.Exists(ArchivoRtu))
                {
                    string json = File.ReadAllText(ArchivoRtu);
                    var cargados = JsonSerializer.Deserialize<List<Contribuyente>>(json);
                    if (cargados != null)
                    {
                        _contribuyentes = cargados;
                    }
                }
            }
            catch { }
        }
    }
}