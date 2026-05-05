using Application.DTO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services.Reports.Components
{
    /// <summary>
    /// Reusable PDF footer component.
    /// Renders: copyright (left) | page numbers (centre) | generated date (right).
    /// </summary>
    public static class PdfFooterComponent
    {
        public static void Build(IContainer container, ReportFooter footer)
        {
            container
                .BorderTop(1).BorderColor("#CCCCCC")
                .PaddingTop(6)
                .Row(row =>
                {
                    // Copyright
                    row.RelativeItem()
                        .Text(footer.CopyrightText)
                        .FontSize(8);

                    // Page numbers (centre)
                    if (footer.ShowPageNumbers)
                    {
                        row.RelativeItem().AlignCenter().Text(x =>
                        {
                            x.Span("Page ").FontSize(8);
                            x.CurrentPageNumber().FontSize(8);
                            x.Span(" of ").FontSize(8);
                            x.TotalPages().FontSize(8);
                        });
                    }

                    // Generated timestamp (right)
                    row.RelativeItem().AlignRight().Column(col =>
                    {
                        col.Item()
                            .Text($"Generated: {footer.GeneratedAt:yyyy-MM-dd HH:mm}")
                            .FontSize(8);

                        if (!string.IsNullOrWhiteSpace(footer.GeneratedBy))
                        {
                            col.Item()
                                .AlignRight()
                                .Text($"By: {footer.GeneratedBy}")
                                .FontSize(8);
                        }
                    });
                });
        }
    }
}
