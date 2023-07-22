namespace XReports.Excel.Writers
{
    /// <summary>
    /// Options for <see cref="EpplusWriter"/>.
    /// </summary>
    public class EpplusWriterOptions
    {
        /// <summary>
        /// Gets or sets name of Excel worksheet to create and write to.
        /// </summary>
        public string WorksheetName { get; set; } = "Data";

        /// <summary>
        /// Gets or sets 1-based row number to start writing report at.
        /// </summary>
        public int StartRow { get; set; } = 1;

        /// <summary>
        /// Gets or sets 1-based column number to start writing report at.
        /// </summary>
        public int StartColumn { get; set; } = 1;
    }
}
