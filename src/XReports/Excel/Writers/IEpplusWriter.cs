using System.IO;
using OfficeOpenXml;
using XReports.Table;

namespace XReports.Excel.Writers
{
    /// <summary>
    /// Interface for writer of Excel report using EPPlus library.
    /// </summary>
    public interface IEpplusWriter
    {
        /// <summary>
        /// Writes Excel report to file.
        /// </summary>
        /// <param name="table">Excel report table.</param>
        /// <param name="fileName">File name to save report to.</param>
        void WriteToFile(IReportTable<ExcelReportCell> table, string fileName);

        /// <summary>
        /// Writes Excel report to memory stream.
        /// </summary>
        /// <param name="table">Excel report table.</param>
        /// <returns>Memory stream with Excel report content.</returns>
        Stream WriteToStream(IReportTable<ExcelReportCell> table);

        /// <summary>
        /// Writes Excel report to stream.
        /// </summary>
        /// <param name="table">Excel report table.</param>
        /// <param name="stream">Stream to write Excel report to.</param>
        void WriteToStream(IReportTable<ExcelReportCell> table, Stream stream);

        /// <summary>
        /// Writes report to Excel worksheet.
        /// </summary>
        /// <param name="table">Report table to write.</param>
        /// <param name="worksheet">Excel worksheet to write to.</param>
        /// <param name="row">1-based row number to start writing at.</param>
        /// <param name="column">1-based column number to start writing at.</param>
        /// <returns>Excel address of report.</returns>
        ExcelAddress WriteToWorksheet(IReportTable<ExcelReportCell> table, ExcelWorksheet worksheet, int row, int column);
    }
}
