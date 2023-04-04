using XReports.Table;

namespace XReports.Converter
{
    /// <summary>
    /// Interface for converter of report with cells of type <see cref="ReportCell"/> to report with cells of type <typeparamref name="TResultReportCell"></typeparamref>.
    /// </summary>
    /// <typeparam name="TResultReportCell">Type of cells to convert to.</typeparam>
    public interface IReportConverter<out TResultReportCell>
        where TResultReportCell : ReportCell
    {
        /// <summary>
        /// Converts report with cells of type <see cref="ReportCell"/> to report with cells of type <typeparamref name="TResultReportCell"></typeparamref>.
        /// </summary>
        /// <param name="table">Report table to convert.</param>
        /// <returns>Report table with converted cells.</returns>
        IReportTable<TResultReportCell> Convert(IReportTable<ReportCell> table);
    }
}
