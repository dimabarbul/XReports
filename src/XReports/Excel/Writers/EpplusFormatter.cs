using OfficeOpenXml;
using XReports.Table;

namespace XReports.Excel.Writers
{
    /// <summary>
    /// Formatter used by <see cref="EpplusWriter"/> that handles one type of report property. Formats only cells that have property of <typeparamref name="TProperty"/>. Derived types are ignored.
    /// </summary>
    /// <typeparam name="TProperty">Type of property to handle.</typeparam>
    public abstract class EpplusFormatter<TProperty> : IEpplusFormatter
        where TProperty : IReportCellProperty
    {
        /// <inheritdoc />
        public void Format(ExcelRange excelRange, ExcelReportCell cell)
        {
            if (!cell.TryGetProperty(out TProperty property))
            {
                return;
            }

            this.Format(excelRange, cell, property);
        }

        /// <summary>
        /// Formats Excel cell range.
        /// </summary>
        /// <param name="excelRange">Excel cell range to format.</param>
        /// <param name="cell">Report cell to take format from.</param>
        /// <param name="property">Report cell property of type that the formatter can handle.</param>
        protected abstract void Format(ExcelRange excelRange, ExcelReportCell cell, TProperty property);
    }
}
