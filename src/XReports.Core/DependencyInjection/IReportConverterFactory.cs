using XReports.Converter;
using XReports.Table;

namespace XReports.DependencyInjection
{
    /// <summary>
    /// Interface for factory that provides report converters that produce report with cells of type <typeparamref name="TReportCell"/>.
    /// </summary>
    /// <typeparam name="TReportCell">Type of cells of report that converters will convert to.</typeparam>
    public interface IReportConverterFactory<out TReportCell>
        where TReportCell : ReportCell, new()
    {
        /// <summary>
        /// Returns report converter by name.
        /// </summary>
        /// <param name="name">Report converter name.</param>
        /// <returns>Report converter.</returns>
        IReportConverter<TReportCell> Get(string name);
    }
}
