using Application.DTO;
using ClosedXML.Excel;

namespace Infrastructure.Services.Reports.Components
{
    /// <summary>
    /// Reusable Excel table component.
    /// Writes a styled table (branded header row + alternating data rows) and
    /// auto-fits all columns. Returns the next available row number.
    /// </summary>
    public static class ExcelTableComponent
    {
        public static int Build(IXLWorksheet ws, ReportTable table, int startRow)
        {
            int colCount = table.Columns.Count;
            int row = startRow;

            // ── Header row ────────────────────────────────────────────────────
            for (int c = 0; c < colCount; c++)
            {
                var cell = ws.Cell(row, c + 1);
                cell.Value = table.Columns[c].Header;
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontSize = 10;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml(ReportBrand.Primary);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.OutsideBorderColor = XLColor.FromHtml(ReportBrand.Primary);
            }
            row++;

            // ── Data rows ─────────────────────────────────────────────────────
            for (int r = 0; r < table.Data.Count; r++)
            {
                var dataRow = table.Data[r];
                var bg = r % 2 == 0 ? XLColor.White : XLColor.FromHtml(ReportBrand.Light);

                for (int c = 0; c < dataRow.Count; c++)
                {
                    var cell = ws.Cell(row, c + 1);
                    cell.Value = dataRow[c] ?? string.Empty;
                    cell.Style.Fill.BackgroundColor = bg;
                    cell.Style.Font.FontSize = 9;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#CCCCCC");
                }
                row++;
            }

            // ── Auto-fit columns ──────────────────────────────────────────────
            ws.Columns(1, colCount).AdjustToContents();

            return row;
        }
    }
}
