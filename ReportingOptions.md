# .NET Core Reporting Guide — Enterprise Blazor Applications

> © Francois Mugorozi @ francoismugorozi@gmail.com

---

## Overview

Reporting in enterprise .NET applications falls into three distinct approaches.
Choosing the wrong one creates maintenance debt fast — over-engineered for
simple exports, or under-powered for complex managed reports.

---

## Part 1 — Options Landscape

### Approach A — Programmatic / Code-First

You define the report structure entirely in C#. No external files, no designer
tools.

---

#### A1. QuestPDF

**What it is:** Fluent C# API for generating PDFs. Layout defined in code using
a box model similar to CSS flexbox.

**When to use:**

- Professional PDF reports (invoices, summaries, certificates)
- Layout is stable and developer-owned
- You need pixel control over design
- Clean architecture — output as `byte[]` from a service

**Pros:**

- Open source (MIT for community)
- Cross-platform, no native dependencies
- Excellent documentation and active maintenance
- Strongly typed — report structure refactors with your code
- Supports images, tables, charts (via SkiaSharp), page headers/footers, page
  numbers

**Cons:**

- Non-developers cannot edit layouts
- Complex designs require significant C# code
- No visual preview during design — iterate via run cycle

---

#### A2. ClosedXML

**What it is:** .NET library for creating and manipulating Excel `.xlsx` files
programmatically.

**When to use:**

- Tabular/statistical data exports users need to interact with
- Data the user will filter, sort, or chart themselves in Excel
- Bulk data exports (thousands of rows)

**Pros:**

- Open source (MIT)
- Full Excel feature support: tables, formulas, styles, charts, named ranges
- Fast and memory-efficient
- No Excel installation required on server

**Cons:**

- Excel format only (use alongside QuestPDF for PDF needs)
- No visual report layout — purely data-grid oriented
- Complex charts require additional work

---

### Approach B — Template-Based Rendering

Report layout is defined in a template file (Razor, HTML, or proprietary
format). Rendered at runtime with injected data.

---

#### B1. Razor Component → HTML → PDF (Playwright / PuppeteerSharp)

**What it is:** Define report layout as a `.razor` component. Render it
server-side to HTML string using `HtmlRenderer`. Convert HTML to PDF via
headless Chromium.

**When to use:**

- Report layout benefits from CSS styling and component reuse
- Designers/front-end developers maintain the layout (not C# devs)
- You want consistency between web UI appearance and PDF output
- Complex charts already rendered in browser (Chart.js, ApexCharts)

**Pros:**

- Full CSS control — pixel-perfect branding
- Razor components are reusable and composable
- Team members already know Razor
- Charts and complex visuals handled naturally via HTML/CSS

**Cons:**

- Playwright adds ~130MB Chromium runtime to your deployment
- Slower than QuestPDF (browser launch overhead ~1–2s per report vs ~50ms)
- HTML→PDF fidelity is not always 1:1 (page breaks, print CSS quirks)
- Adds infrastructure complexity (headless browser on server)

---

#### B2. FastReport Open Source

**What it is:** `.frx` XML-based report template files designed visually in
FastReport Designer, rendered at runtime in .NET.

**When to use:**

- Business analysts or non-developers own and maintain report templates
- You need pixel-perfect paginated reports (invoices, formal documents)
- Reports are numerous and managed as assets independently of code

**Pros:**

- Free open source edition available
- Visual designer — non-developers can edit templates
- Rich data binding, grouping, sub-reports
- Exports to PDF, HTML, Excel, Word, CSV

**Cons:**

- Designer is Windows-only desktop app
- `.frx` files are XML — hard to diff/review in Git
- Open source edition lacks PDF export (needs plugin)
- Learning curve for the designer tool
- Less idiomatic in clean architecture — runtime template loading needs care

---

### Approach C — Third-Party Full Platforms / Designer Tools

Complete reporting platforms: visual designers, viewers, schedulers,
multi-tenant management.

---

#### C1. Telerik Reporting

**What it is:** Full commercial reporting platform from Progress. Includes
report designer (standalone + web-based), Blazor viewer component, REST API
report server, and multi-format export.

**When to use:**

- Enterprise apps where end users design and manage their own reports
- Blazor-native viewer required (embedded in your UI)
- Scheduled report delivery needed
- Budget available for commercial licensing

**Pros:**

- Native Blazor viewer component
- Web-based designer end users can use
- Excellent .NET Core support
- Wide export formats (PDF, Excel, Word, CSV, Image)
- Active support from Progress

**Cons:**

- Commercial license required (per developer)
- Vendor lock-in — reports tied to Telerik format (`.trdp`, `.trdx`)
- Overkill for simple export needs
- Heavy dependency to introduce early in a project

---

#### C2. Crystal Reports (SAP)

**What it is:** One of the longest-standing report designers in the enterprise
world, now owned by SAP. Reports are designed in a visual `.rpt` designer and
rendered via a runtime engine.

**⚠️ Critical .NET Core Compatibility Warning:** As of 2024, the Crystal Reports
runtime engine **only officially supports .NET Framework 4.8 — not .NET Core,
.NET 6, 7, 8, or 9**. Running Crystal Reports in a modern .NET Core application
requires workarounds (see below). The 32-bit runtime was discontinued in
December 2025. Crystal Reports 2025 (64-bit) is the current release from SAP.

**Workarounds for .NET Core:**

- **CrystalReportsRunner** (open source) — runs the Crystal Reports engine in a
  separate .NET Framework 4.8 process and communicates via named pipes. Brittle
  and operationally complex.
- **CrystalCMD** — external console tool that accepts `.rpt` files and data and
  returns PDF output. Decoupled but adds a process boundary.
- **Hybrid approach** — keep a .NET Framework 4.8 sidecar service alongside your
  .NET Core app, exposing report generation over HTTP.

**When to use:**

- You are migrating an existing legacy application that already has a large
  catalog of `.rpt` report templates
- The organization has Crystal Reports licenses and trained report designers
- Short-term: you cannot afford a full reporting rewrite immediately

**When NOT to use:**

- Any greenfield .NET Core / .NET 6+ project — there is no native support
- Containerized (Docker/Kubernetes) deployments — the runtime has significant
  Windows dependencies
- When you want clean architecture — the runtime coupling is significant

**Pros:**

- Mature, feature-rich designer with decades of refinement
- Business users are often already trained on it
- Rich data connectivity (ODBC, OLE DB, SQL, XML, etc.)
- Strong pixel-perfect paginated report output
- SAP Crystal Reports 2025 brings 64-bit support and continued investment

**Cons:**

- **No native .NET Core support** — workarounds only
- Windows-only runtime — no Linux, no Docker
- Heavy runtime installation required on every server
- Licensing costs for Crystal Reports Designer + runtime redistribution
- `.rpt` files are binary — not version-control friendly
- SAP's investment pace in this product is slow compared to modern alternatives
- Workarounds (named pipes, sidecar processes) add operational fragility

**Verdict:** Crystal Reports is a **legacy migration tool**, not a greenfield
choice. If you have existing `.rpt` assets, plan a migration path to modern
alternatives rather than deepening the dependency.

---

#### C3. SSRS / RDLC

**What it is:** Microsoft's native reporting stack. RDLC = embedded report
definition (free, deployable), SSRS = full report server (requires SQL Server
license).

**When to use (RDLC):**

- Microsoft-stack shops already using SQL Server
- Standardised report definitions needed across teams
- Reports consumed programmatically without a viewer

**When to use (SSRS):**

- Full report server with portal, scheduling, and email delivery
- Large org with many stakeholders accessing reports directly
- Already licensed for SQL Server Enterprise

**Pros:**

- RDLC is free, ships with .NET
- Familiar to enterprise Microsoft shops
- SSRS has mature scheduling, subscription, and security model

**Cons:**

- RDLC designer is Visual Studio only (aging experience)
- SSRS is Windows Server + SQL Server — not cloud/container-native
- Poor Blazor integration (iframe-based viewer)
- `.rdlc` XML templates are hard to version and maintain
- Microsoft investment in SSRS is minimal — it's in maintenance mode

---

#### C4. DevExpress / Stimulsoft / Bold Reports

**What it is:** Commercial full-stack reporting suites similar to Telerik. Each
includes visual designers, viewers, export engines, and some form of report
server.

|                    | DevExpress    | Stimulsoft    | Bold Reports     |
| ------------------ | ------------- | ------------- | ---------------- |
| Blazor Viewer      | Yes           | Yes           | Yes              |
| Web Designer       | Yes           | Yes           | Yes              |
| SSRS-compatible    | Partial       | Partial       | Yes (RDL)        |
| Self-hosted server | Yes           | Yes           | Yes (Docker/K8s) |
| Pricing            | Per developer | Per developer | Per deployment   |
| Open source tier   | No            | No            | No               |

**When to use:** Same tier as Telerik — choose based on existing vendor
relationships, designer UX preference, or specific compatibility needs (Bold
Reports if migrating from SSRS, as it is RDL-compatible and Docker/K8s
friendly).

---

## Part 2 — Comparison Summary

| Option                         | Layout Owner     | PDF | Excel | Designer            | Blazor Native   | OSS     | .NET Core     | Complexity |
| ------------------------------ | ---------------- | --- | ----- | ------------------- | --------------- | ------- | ------------- | ---------- |
| QuestPDF                       | Developer        | ✅  | ❌    | None                | Via `byte[]`    | ✅      | ✅            | Low        |
| ClosedXML                      | Developer        | ❌  | ✅    | None                | Via `byte[]`    | ✅      | ✅            | Low        |
| Razor → PDF                    | Dev / Designer   | ✅  | ❌    | VS Code / Rider     | Native `.razor` | ✅\*    | ✅            | Medium     |
| FastReport OSS                 | Business Analyst | ✅  | ✅    | Desktop (Win)       | Via `byte[]`    | ✅      | ✅            | Medium     |
| Crystal Reports                | Business Analyst | ✅  | ✅    | Desktop (Win)       | Via workaround  | ❌      | ⚠️ Workaround | High       |
| Telerik Reporting              | Business Analyst | ✅  | ✅    | Web + Desktop       | ✅ Native       | ❌      | ✅            | High       |
| SSRS / RDLC                    | Dev / BA         | ✅  | ✅    | VS / Report Builder | ❌ (iframe)     | Partial | ⚠️ RDLC only  | High       |
| DevExpress / Stimulsoft / Bold | Business Analyst | ✅  | ✅    | Web + Desktop       | ✅              | ❌      | ✅            | High       |

> \*OSS dependencies only — Playwright itself is MIT

---

## Part 3 — Implementation Guides

---

### Guide 1 — QuestPDF

#### Step 1 — Install

```
dotnet add package QuestPDF
```

#### Step 2 — Set License (required even for community tier)

```csharp
// Program.cs
QuestPDF.Settings.License = LicenseType.Community;
```

#### Step 3 — Define the Report Data Model (Domain / Application layer)

```csharp
// Application/Reports/Models/SalesReportData.cs
public record SalesReportData(
    string Title,
    string Period,
    string OrganizationName,
    IReadOnlyList<SalesReportRow> Rows
);

public record SalesReportRow(string Product, int Quantity, decimal Revenue);
```

#### Step 4 — Define the Report Document (Infrastructure layer)

```csharp
// Infrastructure/Reports/Documents/SalesReportDocument.cs
public class SalesReportDocument : IDocument
{
    private readonly SalesReportData _data;
    public SalesReportDocument(SalesReportData data) => _data = data;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.Header().Element(ComposeHeader);
            page.Content().PaddingTop(10).Element(ComposeTable);
            page.Footer().Element(ComposeFooter);
        });
    }

    void ComposeHeader(IContainer c) { /* logo, title, period */ }
    void ComposeTable(IContainer c) { /* table headers + data rows */ }
    void ComposeFooter(IContainer c) { /* page numbers, generated date */ }
}
```

#### Step 5 — Define the Report Service

```csharp
// Application/Reports/ISalesReportService.cs
public interface ISalesReportService {
    Task<byte[]> GeneratePdfAsync(SalesReportData data, CancellationToken ct);
}

// Infrastructure/Reports/SalesReportService.cs
public class SalesReportService : ISalesReportService {
    public Task<byte[]> GeneratePdfAsync(SalesReportData data, CancellationToken ct) {
        var document = new SalesReportDocument(data);
        var bytes = document.GeneratePdf();
        return Task.FromResult(bytes);
    }
}
```

#### Step 6 — Register in DI

```csharp
// Infrastructure/DependencyInjection.cs
services.AddScoped<ISalesReportService, SalesReportService>();
```

#### Step 7 — Blazor Component

```razor
@* Pages/Reports/SalesReport.razor *@
@inject ISalesReportService ReportService
@inject IJSRuntime JS

<button @onclick="ExportPdf" disabled="@_loading">
    @(_loading ? "Generating..." : "Export PDF")
</button>

@code {
    bool _loading;

    async Task ExportPdf() {
        _loading = true;
        try {
            var data = await LoadReportDataAsync(); // your query/mediator call
            var bytes = await ReportService.GeneratePdfAsync(data, CancellationToken.None);
            await JS.InvokeVoidAsync("downloadFile", "sales-report.pdf", "application/pdf", bytes);
        } finally {
            _loading = false;
        }
    }
}
```

#### Step 8 — JS Download Helper

Add once in `app.js` or inline in `index.html`:

```javascript
window.downloadFile = (fileName, mimeType, bytes) => {
	const blob = new Blob([new Uint8Array(bytes)], { type: mimeType });
	const url = URL.createObjectURL(blob);
	const a = document.createElement("a");
	a.href = url;
	a.download = fileName;
	a.click();
	URL.revokeObjectURL(url);
};
```

---

### Guide 2 — ClosedXML (Excel Export)

#### Step 1 — Install

```
dotnet add package ClosedXML
```

#### Step 2 — Define the Export Service

```csharp
// Application/Reports/ISalesExcelExportService.cs
public interface ISalesExcelExportService {
    byte[] Generate(IEnumerable<SalesReportRow> rows);
}

// Infrastructure/Reports/SalesExcelExportService.cs
public class SalesExcelExportService : ISalesExcelExportService {
    public byte[] Generate(IEnumerable<SalesReportRow> rows) {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Sales");

        // Style the header row
        var headers = new[] { "Product", "Quantity", "Revenue" };
        for (int i = 0; i < headers.Length; i++) {
            var cell = ws.Cell(1, i + 1);
            cell.Value = headers[i];
            /* apply bold, background color, font color */
        }

        // Populate data rows
        int row = 2;
        foreach (var item in rows) {
            ws.Cell(row, 1).Value = item.Product;
            ws.Cell(row, 2).Value = item.Quantity;
            ws.Cell(row, 3).Value = item.Revenue;
            /* apply currency format to Revenue column */
            row++;
        }

        // Make it a filterable Excel table
        ws.RangeUsed().CreateTable("SalesData");
        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
```

#### Step 3 — Register in DI

```csharp
services.AddScoped<ISalesExcelExportService, SalesExcelExportService>();
```

#### Step 4 — Blazor Component

```razor
@inject ISalesExcelExportService ExcelService
@inject IJSRuntime JS

<button @onclick="ExportExcel">Export Excel</button>

@code {
    async Task ExportExcel() {
        var rows = await LoadRowsAsync(); // mediator / service call
        var bytes = ExcelService.Generate(rows);
        await JS.InvokeVoidAsync("downloadFile",
            $"sales-{DateTime.Today:yyyy-MM-dd}.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            bytes);
    }
}
```

> Same `window.downloadFile` JS helper from Guide 1 is reused.

---

### Guide 3 — Razor Component → PDF (Playwright)

Use this when the report layout is complex enough that you want CSS/Razor over
C# fluent code.

#### Step 1 — Install

```
dotnet add package Microsoft.Playwright
dotnet add package Microsoft.AspNetCore.Components
```

After install, download Chromium:

```
playwright install chromium
```

On Linux servers / Docker, add to your Dockerfile:

```dockerfile
RUN playwright install chromium --with-deps
```

#### Step 2 — Create the Report as a Razor Component

```razor
@* Infrastructure/Reports/Templates/SalesReportTemplate.razor *@
@* Pure layout component — no @page directive *@

<html>
<head>
    <style>
        body { font-family: Arial, sans-serif; }
        table { width: 100%; border-collapse: collapse; }
        .page-break { page-break-after: always; }
        /* all print-optimized CSS here */
    </style>
</head>
<body>
    <header>
        <h1>@Data.Title</h1>
        <p>@Data.Period</p>
    </header>
    <table>
        <thead>
            <tr><th>Product</th><th>Qty</th><th>Revenue</th></tr>
        </thead>
        <tbody>
            @foreach (var row in Data.Rows) {
                <tr>
                    <td>@row.Product</td>
                    <td>@row.Quantity</td>
                    <td>@row.Revenue.ToString("C")</td>
                </tr>
            }
        </tbody>
    </table>
</body>
</html>

@code {
    [Parameter] public SalesReportData Data { get; set; } = default!;
}
```

#### Step 3 — Razor Renderer Service (Infrastructure)

```csharp
// Infrastructure/Reports/RazorReportRenderer.cs
public class RazorReportRenderer
{
    private readonly IServiceProvider _sp;

    public async Task<string> RenderAsync<TComponent>(Dictionary<string, object?> parameters)
        where TComponent : IComponent
    {
        await using var htmlRenderer = new HtmlRenderer(_sp, NullLoggerFactory.Instance);

        return await htmlRenderer.Dispatcher.InvokeAsync(async () => {
            var paramView = ParameterView.FromDictionary(parameters);
            var output = await htmlRenderer.RenderComponentAsync<TComponent>(paramView);
            return output.ToHtmlString();
        });
    }
}
```

> `HtmlRenderer` is available in `Microsoft.AspNetCore.Components.Web` — .NET 8+

#### Step 4 — PDF Conversion Service (Infrastructure)

```csharp
// Infrastructure/Reports/PlaywrightPdfService.cs
public class PlaywrightPdfService : IAsyncDisposable
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    public async Task InitAsync() {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
    }

    public async Task<byte[]> RenderHtmlToPdfAsync(string html) {
        var page = await _browser!.NewPageAsync();
        await page.SetContentAsync(html, new() { WaitUntil = WaitUntilState.NetworkIdle });
        var pdf = await page.PdfAsync(new() {
            Format = PaperFormat.A4,
            PrintBackground = true,
            MarginTop = "1.5cm",
            MarginBottom = "1.5cm",
            MarginLeft = "1.5cm",
            MarginRight = "1.5cm"
        });
        await page.CloseAsync();
        return pdf;
    }

    public async ValueTask DisposeAsync() { /* dispose browser and playwright */ }
}
```

#### Step 5 — Compose in Report Service

```csharp
public class SalesRazorReportService : ISalesReportService
{
    private readonly RazorReportRenderer _renderer;
    private readonly PlaywrightPdfService _pdf;

    public async Task<byte[]> GeneratePdfAsync(SalesReportData data, CancellationToken ct) {
        var html = await _renderer.RenderAsync<SalesReportTemplate>(
            new Dictionary<string, object?> { ["Data"] = data });
        return await _pdf.RenderHtmlToPdfAsync(html);
    }
}
```

#### Step 6 — Register (singleton for browser, scoped for report service)

```csharp
// Infrastructure/DependencyInjection.cs
builder.Services.AddSingleton<PlaywrightPdfService>(sp => {
    var svc = new PlaywrightPdfService();
    svc.InitAsync().GetAwaiter().GetResult(); // init once at startup
    return svc;
});
builder.Services.AddScoped<RazorReportRenderer>();
builder.Services.AddScoped<ISalesReportService, SalesRazorReportService>();
```

#### Step 7 — Blazor Component

```razor
@inject ISalesReportService ReportService
@inject IJSRuntime JS

<button @onclick="ExportPdf">Export PDF</button>

@code {
    async Task ExportPdf() {
        var data = await LoadDataAsync();
        var bytes = await ReportService.GeneratePdfAsync(data, CancellationToken.None);
        await JS.InvokeVoidAsync("downloadFile", "report.pdf", "application/pdf", bytes);
    }
}
```

> The Blazor component is completely decoupled from which approach generates the
> PDF — it only ever calls `ISalesReportService`. You can swap QuestPDF ↔
> Razor/Playwright without touching a single component.

---

## Part 4 — Recommendations

### Start here

```
PDF reports        → QuestPDF
Excel exports      → ClosedXML
Both in same app   → Both, behind a shared IReportService abstraction
```

QuestPDF + ClosedXML covers the majority of enterprise reporting without adding
any external runtime dependencies. Both are open source, cross-platform, and fit
cleanly into a layered architecture. This is your **default choice for any
greenfield .NET Core project**.

### Move to Razor → PDF when

- Your reports have complex branding that is faster to express in CSS than
  QuestPDF fluent code
- Front-end developers or designers own the report layout
- You are already using component-based design patterns and want reports to
  match your UI system
- Accept the tradeoff: Chromium on your server, slower generation (~1–2s per
  report vs ~50ms QuestPDF)

### Move to FastReport / Telerik / Bold Reports when

- Business analysts or non-developers need to own and modify report templates
  without developer involvement
- You need a built-in report viewer embedded in your Blazor UI (not just file
  download)
- Scheduled report delivery is a product requirement
- Budget is not a constraint

### Avoid Crystal Reports for new projects

Crystal Reports has no native .NET Core support. Every workaround (named pipes,
sidecar processes, CrystalCMD) adds operational fragility. If you have existing
`.rpt` assets, treat it as a **migration target** — inventory your reports,
prioritize by complexity, and migrate progressively to QuestPDF (for
developer-owned layouts) or FastReport (for business-analyst-owned templates).
Do not deepen the Crystal Reports dependency in a .NET Core application.

### Avoid SSRS unless

- You are in a pure Microsoft on-premise shop already running SQL Server
  Enterprise
- The organization already operates an SSRS server with an existing report
  catalog
- You have zero Blazor viewer requirement (SSRS viewer in Blazor is iframe-only)

### The interface is your safety net

Regardless of which approach you choose, define the abstraction in your
Application layer:

```csharp
public interface IReportService {
    Task<byte[]> GeneratePdfAsync(ReportData data, CancellationToken ct);
    byte[] GenerateExcelExport(IEnumerable<ReportRow> rows);
}
```

The Blazor components only ever call `IReportService`. The implementation —
QuestPDF, Razor/Playwright, FastReport — lives exclusively in Infrastructure.
You can swap or upgrade the engine without touching a single component.

---

_© Francois Mugorozi @ francoismugorozi@gmail.com_
