using Application.DTO;

/// <summary>
/// Maps a collection of <typeparamref name="T"/> to a complete <see cref="ReportOptions"/>.
///
/// Why templates instead of a service per report?
///   - Templates are plain objects — just 'new CustomerListTemplate()'. No DI registration needed.
///   - Adding a new report = add one file. No interface + implementation pair.
///   - The caller provides the data; the template handles the layout.
///
/// Usage:
///   var options = new CustomerListTemplate(filter).Build(customers);
///   var bytes   = ReportGenerator.GeneratePdf(options);
/// </summary>
public interface IReportTemplate<T>
{
    ReportOptions Build(IEnumerable<T> data);
}