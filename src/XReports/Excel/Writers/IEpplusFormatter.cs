using OfficeOpenXml;

namespace XReports.Excel.Writers
{
    /// <summary>
    /// Interface for formatter used by <see cref="EpplusWriter"/>.
    /// </summary>
    public interface IEpplusFormatter
    {
        /// <summary>
        /// Formats Excel cell range.
        /// </summary>
        /// <param name="excelRange">Excel range to format.</param>
        /// <param name="cell">Report cell to take format from.</param>
        void Format(ExcelRange excelRange, ExcelReportCell cell);
    }
}
