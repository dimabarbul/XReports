namespace XReports.Schema
{
    /// <summary>
    /// Complex header cell.
    /// </summary>
    public class ComplexHeaderCell
    {
        /// <summary>
        /// Gets or sets a value indicating whether the cell is was created from complex header groups (if true) or it was a report column header cell (if false).
        /// </summary>
        public bool IsComplexHeaderCell { get; set; }

        /// <summary>
        /// Gets or sets content of the cell.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets how many columns the cell spans.
        /// </summary>
        public int ColumnSpan { get; set; }

        /// <summary>
        /// Gets or sets how many rows the cell spans.
        /// </summary>
        public int RowSpan { get; set; }
    }
}
