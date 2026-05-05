namespace Application.DTO
{
    // ── Enums ────────────────────────────────────────────────────────────────
    public enum ReportFormat { Pdf, Excel }
    public enum ReportOrientation { Portrait, Landscape }

    // ── Building blocks ───────────────────────────────────────────────────────

    /// <summary>Defines a single column in the report table.</summary>
    public class ReportColumn
    {
        public string Header { get; set; } = string.Empty;
        /// <summary>Relative width (0 = auto). Used to weight columns against each other.</summary>
        public int Width { get; set; } = 0;
    }

    /// <summary>Table data: columns + rows of string values (all formatting is done in the renderer).</summary>
    public class ReportTable
    {
        public List<ReportColumn> Columns { get; set; } = [];
        public List<List<string>> Data { get; set; } = [];
    }

    /// <summary>Report header metadata shown at the top of every page.</summary>
    public class ReportHeader
    {
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? Description { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
    }

    /// <summary>Report footer shown at the bottom of every page.</summary>
    public class ReportFooter
    {
        public string CopyrightText { get; set; } = $"© {DateTime.Now.Year} CRM Demo";
        public bool ShowPageNumbers { get; set; } = true;
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public string? GeneratedBy { get; set; }
    }

    // ── Composition ───────────────────────────────────────────────────────────

    /// <summary>
    /// Everything the report generator needs to render a report.
    /// Build this in a report service and pass it to <c>IReportGenerator</c>.
    /// </summary>
    public class ReportOptions
    {
        public ReportHeader Header { get; set; } = new();
        public ReportTable Table { get; set; } = new();
        public ReportFooter Footer { get; set; } = new();
        public ReportOrientation Orientation { get; set; } = ReportOrientation.Portrait;
    }

    /// <summary>
    /// Returned by report services. Contains the file bytes and the metadata
    /// needed to push the file to the browser (name + content type).
    /// </summary>
    public record ReportFile(byte[] Bytes, string FileName, string ContentType);
}
