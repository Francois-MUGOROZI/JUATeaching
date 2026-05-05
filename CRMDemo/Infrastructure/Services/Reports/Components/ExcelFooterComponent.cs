using Application.DTO;
using ClosedXML.Excel;

namespace Infrastructure.Services.Reports.Components
{
    /// <summary>
    /// Reusable Excel footer component.
    /// Writes generated timestamp (and optional author) below the table.
    /// </summary>
    public static class ExcelFooterComponent
    {
        public static void Build(IXLWorksheet ws, ReportFooter footer, int startRow)
        {
            int row = startRow + 1; // blank row gap

            var genCell = ws.Cell(row, 1);
            genCell.Value = $"Generated: {footer.GeneratedAt:yyyy-MM-dd HH:mm}";
            genCell.Style.Font.FontSize = 8;
            genCell.Style.Font.Italic = true;

            if (!string.IsNullOrWhiteSpace(footer.GeneratedBy))
            {
                row++;
                var byCell = ws.Cell(row, 1);
                byCell.Value = $"By: {footer.GeneratedBy}";
                byCell.Style.Font.FontSize = 8;
                byCell.Style.Font.Italic = true;
            }

            row++;
            var copyrightCell = ws.Cell(row, 1);
            copyrightCell.Value = footer.CopyrightText;
            copyrightCell.Style.Font.FontSize = 8;
        }
    }
}
