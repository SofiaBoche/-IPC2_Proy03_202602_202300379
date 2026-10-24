using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace WebFrontend.Pages
{
    public class ReportesModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "http://localhost:5234/api/";

        public List<FacturaDto> ListaFacturas { get; set; } = new List<FacturaDto>();
        public int TotalProcesadas { get; set; }
        public int TotalAprobadas { get; set; }
        public int TotalRechazadas { get; set; }

        public ReportesModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task OnGetAsync()
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = await _httpClient.GetFromJsonAsync<List<FacturaDto>>($"{ApiBaseUrl}consulta-datos", options);
                
                if (response != null)
                {
                    ListaFacturas = response;
                    TotalProcesadas = ListaFacturas.Count;
                    TotalAprobadas = ListaFacturas.Count(f => f.EsValida);
                    TotalRechazadas = ListaFacturas.Count(f => !f.EsValida);
                }
            }
            catch
            {
                ListaFacturas = new List<FacturaDto>();
            }
        }

        public async Task<IActionResult> OnPostGenerarPdfAsync()
        {
            await OnGetAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Reporte de Autorización de Facturas").FontSize(20).Bold().FontColor(Colors.Blue.Medium);
                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Text($"Total Procesadas: {TotalProcesadas} | Aprobadas: {TotalAprobadas} | Rechazadas: {TotalRechazadas}").FontSize(12);
                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Referencia").Bold();
                                header.Cell().Text("Emisor").Bold();
                                header.Cell().Text("Receptor").Bold();
                                header.Cell().Text("Total").Bold();
                                header.Cell().Text("Estado").Bold();
                            });

                            foreach (var fac in ListaFacturas)
                            {
                                table.Cell().Text(fac.Referencia);
                                table.Cell().Text(fac.NitEmisor);
                                table.Cell().Text(fac.NitReceptor);
                                table.Cell().Text($"Q {fac.Total:N2}");
                                table.Cell().Text(fac.EsValida ? "Aprobada" : "Rechazada");
                            }
                        });
                    });
                });
            });

            byte[] pdfBytes = pdf.GeneratePdf();
            return File(pdfBytes, "application/pdf", "Reporte_Facturas.pdf");
        }
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