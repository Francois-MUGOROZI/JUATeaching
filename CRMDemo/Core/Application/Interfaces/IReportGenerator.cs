using Application.DTO;

namespace Application.Interfaces
{
    /// <summary>
    /// Renders a <see cref="ReportOptions"/> into a downloadable file.
    /// Format dispatch, filename construction, and content-type are all resolved here.
    /// Callers only provide the layout options, the format, and a base filename.
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Generates a report file. Returns a <see cref="ReportFile"/> with bytes, filename, and content-type.
        /// The filename is: <paramref name="baseName"/>-yyyyMMdd.pdf|xlsx
        /// </summary>
        ReportFile Generate(ReportOptions options, ReportFormat format, string baseName);
    }
}
