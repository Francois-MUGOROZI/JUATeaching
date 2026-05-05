using Application.DTO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services.Reports.Components
{
    /// <summary>
    /// Reusable PDF header component.
    /// Renders: Title → Subtitle → Description → horizontal rule.
    /// Pass a <see cref="ReportHeader"/> and plug it into any page.
    /// </summary>
    public static class PdfHeaderComponent
    {
        public static void Build(IContainer container, ReportHeader header)
        {
            container.PaddingBottom(8).Column(col =>
            {
                // Logo — path owned by ReportBrand (infrastructure concern, not passed from the page)
                if (File.Exists(ReportBrand.LogoPath))
                {
                    col.Item()
                        .Width(100)
                        .Height(60)
                        .Image(ReportBrand.LogoPath)
                        .FitArea();
                    col.Item().PaddingBottom(6);
                }

                // Title
                col.Item()
                    .Text(header.Title)
                    .FontSize(16)
                    .Bold()
                    .FontColor(ReportBrand.Primary);

                // Subtitle (optional)
                if (!string.IsNullOrWhiteSpace(header.Subtitle))
                {
                    col.Item()
                        .PaddingTop(2)
                        .Text(header.Subtitle)
                        .FontSize(11)
                        .FontColor(ReportBrand.Secondary);
                }

                // Description (optional)
                if (!string.IsNullOrWhiteSpace(header.Description))
                {
                    col.Item()
                        .PaddingTop(2)
                        .Text(header.Description)
                        .FontSize(9)
                        .Italic();
                }

                // Separator line
                col.Item()
                    .PaddingTop(6)
                    .LineHorizontal(1)
                    .LineColor(ReportBrand.Primary);
            });
        }
    }
}
