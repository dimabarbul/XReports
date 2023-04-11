namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies that report built from this class is horizontal.
    /// </summary>
    public sealed class HorizontalReportAttribute : ReportAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalReportAttribute"/> class.
        /// </summary>
        public HorizontalReportAttribute()
            : base(ReportType.Horizontal)
        {
        }
    }
}
