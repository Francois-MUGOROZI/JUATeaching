using Application.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services.Reports.Components
{
    /// <summary>
    /// Reusable PDF table component.
    /// Renders a styled table with a branded header row and alternating row colours.
    /// Pass any <see cref="ReportTable"/> and it handles the rest.
    /// </summary>
    public static class PdfTableComponent
    {
        public static void Build(IContainer container, ReportTable table)
        {
            container.Table(t =>
            {
                // ── Column widths ───────────────────────────────────────────
                t.ColumnsDefinition(cols =>
                {
                    foreach (var col in table.Columns)
                    {
                        if (col.Width > 0) cols.RelativeColumn(col.Width);
                        else cols.RelativeColumn();
                    }
                });

                // ── Header row ──────────────────────────────────────────────
                t.Header(header =>
                {
                    foreach (var col in table.Columns)
                    {
                        header.Cell()
                            .Background(ReportBrand.Primary)
                            .Border(1).BorderColor(Colors.Grey.Darken1)
                            .Padding(5)
                            .AlignMiddle()
                            .Text(col.Header)
                            .FontSize(10)
                            .Bold()
                            .FontColor(ReportBrand.White);
                    }
                });

                // ── Data rows ───────────────────────────────────────────────
                for (int rowIdx = 0; rowIdx < table.Data.Count; rowIdx++)
                {
                    var row = table.Data[rowIdx];
                    var bg = rowIdx % 2 == 0 ? ReportBrand.White : ReportBrand.Light;

                    foreach (var cell in row)
                    {
                        t.Cell()
                            .Background(bg)
                            .Border(1).BorderColor(Colors.Grey.Lighten3)
                            .Padding(3)
                            .Text(cell ?? string.Empty)
                            .FontSize(9)
                            .FontColor(Colors.Black);
                    }
                }
            });
        }
    }
}
