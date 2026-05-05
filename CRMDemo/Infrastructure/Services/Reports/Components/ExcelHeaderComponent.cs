using Application.DTO;
using ClosedXML.Excel;

namespace Infrastructure.Services.Reports.Components
{
    public static class ExcelHeaderComponent
    {
        public static int Build(IXLWorksheet ws, ReportHeader header, int startRow = 1)
        {
            int row = startRow;

            // Logo — path owned by ReportBrand (infrastructure concern, not passed from the page)
            if (File.Exists(ReportBrand.LogoPath))
            {
                ws.AddPicture(ReportBrand.LogoPath)
                    .MoveTo(ws.Cell(row, 1))
                    .Scale(0.5);
                row += 5;  // leave space for the image
            }

            // Title
            var titleCell = ws.Cell(row, 1);
            titleCell.Value = header.Title;
            titleCell.Style.Font.Bold = true;
            titleCell.Style.Font.FontSize = 16;
            titleCell.Style.Font.FontColor = XLColor.FromHtml(ReportBrand.Primary);
            row++;

            // Subtitle (optional)
            if (!string.IsNullOrWhiteSpace(header.Subtitle))
            {
                var cell = ws.Cell(row, 1);
                cell.Value = header.Subtitle;
                cell.Style.Font.FontSize = 11;
                cell.Style.Font.FontColor = XLColor.FromHtml(ReportBrand.Secondary);
                row++;
            }

            // Description (optional)
            if (!string.IsNullOrWhiteSpace(header.Description))
            {
                var cell = ws.Cell(row, 1);
                cell.Value = header.Description;
                cell.Style.Font.FontSize = 9;
                cell.Style.Font.Italic = true;
                row++;
            }

            row++; // blank spacer before table
            return row;
        }
    }
}
