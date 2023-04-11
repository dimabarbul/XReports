namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies that report built from this class is vertical.
    /// </summary>
    public sealed class VerticalReportAttribute : ReportAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalReportAttribute"/> class.
        /// </summary>
        public VerticalReportAttribute()
            : base(ReportType.Vertical)
        {
        }
    }
}
