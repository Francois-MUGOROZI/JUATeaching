using Application.DTO;
using Application.Interfaces;
using ClosedXML.Excel;
using Infrastructure.Services.Reports.Components;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services.Reports
{
    /// <summary>
    /// Concrete report generator: QuestPDF for PDFs, ClosedXML for Excel.
    /// Handles format dispatch, filename construction, and content-type — nothing else needs to.
    /// </summary>
    public class ReportGenerator : IReportGenerator
    {
        private static readonly string _dateStamp = DateTime.Now.ToString("yyyyMMdd");

        public ReportGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public ReportFile Generate(ReportOptions options, ReportFormat format, string baseName)
        {
            return format == ReportFormat.Excel
                ? new ReportFile(GenerateExcel(options), $"{baseName}-{DateTime.Now:yyyyMMdd}.xlsx",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                : new ReportFile(GeneratePdf(options), $"{baseName}-{DateTime.Now:yyyyMMdd}.pdf",
                    "application/pdf");
        }

        // ── PDF ───────────────────────────────────────────────────────────────

        private static byte[] GeneratePdf(ReportOptions options)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    var pageSize = options.Orientation == ReportOrientation.Landscape
                        ? PageSizes.A4.Landscape()
                        : PageSizes.A4.Portrait();

                    page.Size(pageSize);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(c => PdfHeaderComponent.Build(c, options.Header));
                    page.Content().PaddingVertical(10).Element(c => PdfTableComponent.Build(c, options.Table));
                    page.Footer().Element(c => PdfFooterComponent.Build(c, options.Footer));
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }

        // ── Excel ─────────────────────────────────────────────────────────────

        private static byte[] GenerateExcel(ReportOptions options)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Report");

            int row = ExcelHeaderComponent.Build(worksheet, options.Header, startRow: 1);
            row = ExcelTableComponent.Build(worksheet, options.Table, row);
            ExcelFooterComponent.Build(worksheet, options.Footer, row);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
