using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ApiBackend.Models;
using ApiBackend.Utils;

namespace ApiBackend.Services
{
    public class DataService
    {
        private static readonly string RtuFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rtu_data.json");
        private static readonly string FacturasFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "facturas_data.json");
        private static readonly string CorrelativosFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correlativos_data.json");

        private static readonly object _lock = new object();

        public List<Contribuyente> ObtenerContribuyentes()
        {
            lock (_lock)
            {
                if (!File.Exists(RtuFilePath)) return new List<Contribuyente>();
                string json = File.ReadAllText(RtuFilePath);
                return JsonSerializer.Deserialize<List<Contribuyente>>(json) ?? new List<Contribuyente>();
            }
        }

        public RespuestaRtuDto ProcesarRtu(List<Contribuyente> listaInput)
        {
            lock (_lock)
            {
                var contribuyentesActuales = ObtenerContribuyentes();
                int nuevos = 0;
                int actualizados = 0;
                int invalidos = 0;

                foreach (var item in listaInput)
                {
                    if (!NitValidator.EsNitValido(item.Nit))
                    {
                        invalidos++;
                        continue;
                    }

                    var existente = contribuyentesActuales.FirstOrDefault(c => c.Nit == item.Nit);
                    if (existente != null)
                    {
                        existente.Nombre = item.Nombre;
                        actualizados++;
                    }
                    else
                    {
                        contribuyentesActuales.Add(new Contribuyente(item.Nit, item.Nombre));
                        nuevos++;
                    }
                }

                File.WriteAllText(RtuFilePath, JsonSerializer.Serialize(contribuyentesActuales, new JsonSerializerOptions { WriteIndented = true }));

                return new RespuestaRtuDto
                {
                    ContribuyentesNuevos = nuevos,
                    ContribuyentesActualizados = actualizados,
                    NitsInvalidos = invalidos
                };
            }
        }

        public List<Factura> ObtenerFacturas()
        {
            lock (_lock)
            {
                if (!File.Exists(FacturasFilePath)) return new List<Factura>();
                string json = File.ReadAllText(FacturasFilePath);
                return JsonSerializer.Deserialize<List<Factura>>(json) ?? new List<Factura>();
            }
        }

        private string GenerarCodigoAprobacion(DateTime fecha)
        {
            string claveFecha = fecha.ToString("yyyyMMdd");
            Dictionary<string, int> correlativos = new Dictionary<string, int>();

            if (File.Exists(CorrelativosFilePath))
            {
                string json = File.ReadAllText(CorrelativosFilePath);
                correlativos = JsonSerializer.Deserialize<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
            }

            int actual = 0;
            if (correlativos.ContainsKey(claveFecha))
            {
                actual = correlativos[claveFecha];
            }

            actual++;
            correlativos[claveFecha] = actual;

            File.WriteAllText(CorrelativosFilePath, JsonSerializer.Serialize(correlativos, new JsonSerializerOptions { WriteIndented = true }));

            return $"{claveFecha}{actual:D8}";
        }

        public ResultadoAutorizacionesDto ProcesarFacturas(List<Factura> facturasInput)
        {
            lock (_lock)
            {
                var contribuyentes = ObtenerContribuyentes();
                var facturasExistentes = ObtenerFacturas();

                int aprobadas = 0;
                int rechazadasEmisor = 0;
                int rechazadasReceptor = 0;

                var detallesAprobacion = new List<DetalleAprobacionDto>();

                foreach (var fac in facturasInput)
                {
                    bool emisorExiste = contribuyentes.Any(c => c.Nit == fac.NitEmisor && NitValidator.EsNitValido(c.Nit));
                    bool receptorExiste = contribuyentes.Any(c => c.Nit == fac.NitReceptor && NitValidator.EsNitValido(c.Nit));

                    if (!emisorExiste)
                    {
                        rechazadasEmisor++;
                        fac.EsValida = false;
                        facturasExistentes.Add(fac);
                        continue;
                    }

                    if (!receptorExiste)
                    {
                        rechazadasReceptor++;
                        fac.EsValida = false;
                        facturasExistentes.Add(fac);
                        continue;
                    }

                    decimal ivaCalculado = Math.Round(fac.Valor * 0.12m, 2, MidpointRounding.AwayFromZero);
                    decimal totalCalculado = fac.Valor + fac.Iva;

                    bool ivaCorrecto = Math.Abs(fac.Iva - ivaCalculado) <= 0.01m;
                    bool totalCorrecto = Math.Abs(fac.Total - totalCalculado) <= 0.01m;
                    bool referenciaUnica = !facturasExistentes.Any(f => f.Referencia == fac.Referencia && f.EsValida);

                    if (ivaCorrecto && totalCorrecto && referenciaUnica)
                    {
                        DateTime fechaHoy = DateTime.Now;
                        string codigo = GenerarCodigoAprobacion(fechaHoy);

                        fac.CodigoAprobacion = codigo;
                        fac.EsValida = true;
                        fac.FechaProcesamiento = fechaHoy;

                        aprobadas++;
                        detallesAprobacion.Add(new DetalleAprobacionDto
                        {
                            Referencia = fac.Referencia,
                            CodigoAprobacion = codigo
                        });
                    }
                    else
                    {
                        fac.EsValida = false;
                    }

                    facturasExistentes.Add(fac);
                }

                File.WriteAllText(FacturasFilePath, JsonSerializer.Serialize(facturasExistentes, new JsonSerializerOptions { WriteIndented = true }));

                return new ResultadoAutorizacionesDto
                {
                    CantidadAutorizacionesAprobadas = aprobadas,
                    CantidadAutorizacionesRechazadasPorNitEmisorInexistente = rechazadasEmisor,
                    CantidadAutorizacionesRechazadasPorNitReceptorInexistente = rechazadasReceptor,
                    DetalleAprobaciones = detallesAprobacion
                };
            }
        }

        public void ResetearSistema()
        {
            lock (_lock)
            {
                if (File.Exists(RtuFilePath)) File.Delete(RtuFilePath);
                if (File.Exists(FacturasFilePath)) File.Delete(FacturasFilePath);
                if (File.Exists(CorrelativosFilePath)) File.Delete(CorrelativosFilePath);
            }
        }
    }
}