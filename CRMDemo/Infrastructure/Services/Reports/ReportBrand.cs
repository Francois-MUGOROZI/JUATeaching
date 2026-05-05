namespace Infrastructure.Services.Reports
{
    /// <summary>Central brand colours, logo path, and other branding elements. Change these to match the real brand.</summary>
    public static class ReportBrand
    {
        public const string Primary = "#1E3A5F"; // Navy blue
        public const string Secondary = "#3B82F6"; // Bright blue
        public const string Light = "#EFF6FF"; // Very light blue (alternating rows)
        public const string White = "#FFFFFF";
        public static string LogoPath => "wwwroot/images/company-logo.png"; // Adjust as needed

    }
}
