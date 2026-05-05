using Application.DTO;
using Application.Interfaces;
using Application.Services.Customers;
using Domain.Entities;

namespace Application.Services.Reports
{
    // ── Contract ──────────────────────────────────────────────────────────────
    // To add a new customer report: add a method to the interface and implement
    // it below — no new files, no new interface/service pair.

    public interface ICustomerReportService
    {
        ReportFile ExportCustomerList(CustomerFilterDTO filter, ReportFormat format);
        ReportFile ExportCustomerSummaryByStatus(ReportFormat format);
    }

    // ── Service ───────────────────────────────────────────────────────────────
    // Pattern per method:
    //   1. Fetch data.  2. Build ReportOptions via a private template.
    //   3. Generate bytes.  4. Return ReportFile.

    public class CustomerReportService : ICustomerReportService
    {
        private readonly ICustomerService _customers;
        private readonly IReportGenerator _generator;

        public CustomerReportService(ICustomerService customers, IReportGenerator generator)
        {
            _customers = customers;
            _generator = generator;
        }

        public ReportFile ExportCustomerList(CustomerFilterDTO filter, ReportFormat format)
        {
            var options = new CustomerListTemplate(filter).Build(_customers.GetAllCustomers(filter));
            return Wrap(options, format, "customer-list");
        }

        public ReportFile ExportCustomerSummaryByStatus(ReportFormat format)
        {
            var options = new CustomerSummaryTemplate().Build(_customers.GetAllCustomers(new CustomerFilterDTO()));
            return Wrap(options, format, "customer-summary");
        }

        private ReportFile Wrap(ReportOptions options, ReportFormat format, string baseName)
            => _generator.Generate(options, format, baseName);

        // ── Templates (private — add one per report type) ─────────────────────

        private sealed class CustomerListTemplate(CustomerFilterDTO? filter = null) : IReportTemplate<Customer>
        {
            private readonly CustomerFilterDTO _filter = filter ?? new CustomerFilterDTO();

            public ReportOptions Build(IEnumerable<Customer> data)
            {
                var list = data.ToList();
                return new ReportOptions
                {
                    Orientation = ReportOrientation.Landscape,
                    Header = new ReportHeader
                    {
                        Title = "Customer List",
                        Subtitle = BuildSubtitle(),
                        Description = $"Total records: {list.Count}",
                        GeneratedAt = DateTime.Now
                    },
                    Table = new ReportTable
                    {
                        Columns =
                        [
                            new ReportColumn { Header = "#",          Width = 1 },
                            new ReportColumn { Header = "First Name", Width = 3 },
                            new ReportColumn { Header = "Last Name",  Width = 3 },
                            new ReportColumn { Header = "Email",      Width = 5 },
                            new ReportColumn { Header = "Phone",      Width = 3 },
                            new ReportColumn { Header = "Status",     Width = 2 },
                            new ReportColumn { Header = "Created",    Width = 3 }
                        ],
                        Data = list.Select((c, i) => new List<string>
                        {
                            (i + 1).ToString(), c.FirstName, c.LastName, c.Email,
                            c.PhoneNumber ?? "-", c.Status.ToString(), c.CreatedAt.ToString("yyyy-MM-dd")
                        }).ToList()
                    },
                    Footer = new ReportFooter { GeneratedAt = DateTime.Now }
                };
            }

            private string BuildSubtitle()
            {
                var parts = new List<string>();
                if (!string.IsNullOrWhiteSpace(_filter.SearchTerm)) parts.Add($"Search: \"{_filter.SearchTerm}\"");
                if (_filter.Status.HasValue) parts.Add($"Status: {_filter.Status}");
                if (_filter.CreatedFrom.HasValue) parts.Add($"From: {_filter.CreatedFrom:yyyy-MM-dd}");
                if (_filter.CreatedTo.HasValue) parts.Add($"To: {_filter.CreatedTo:yyyy-MM-dd}");
                return parts.Count > 0 ? string.Join("  |  ", parts) : "All customers";
            }
        }

        private sealed class CustomerSummaryTemplate : IReportTemplate<Customer>
        {
            public ReportOptions Build(IEnumerable<Customer> data)
            {
                var list = data.ToList();
                var total = list.Count;
                return new ReportOptions
                {
                    Header = new ReportHeader
                    {
                        Title = "Customer Summary by Status",
                        Subtitle = $"As of {DateTime.Now:MMMM dd, yyyy}",
                        Description = $"Total customers: {total}",
                        GeneratedAt = DateTime.Now
                    },
                    Table = new ReportTable
                    {
                        Columns =
                        [
                            new ReportColumn { Header = "Status",     Width = 3 },
                            new ReportColumn { Header = "Count",      Width = 2 },
                            new ReportColumn { Header = "Percentage", Width = 2 }
                        ],
                        Data = list.GroupBy(c => c.Status)
                            .Select(g => new List<string>
                            {
                                g.Key.ToString(),
                                g.Count().ToString(),
                                total > 0 ? $"{g.Count() * 100.0 / total:F1}%" : "0%"
                            }).ToList()
                    },
                    Footer = new ReportFooter { GeneratedAt = DateTime.Now }
                };
            }
        }
    }
}
