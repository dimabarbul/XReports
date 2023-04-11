using System.IO;
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
        /// Writes Excel report to stream.
        /// </summary>
        /// <param name="table">Excel report table.</param>
        /// <returns>Stream with Excel report content.</returns>
        Stream WriteToStream(IReportTable<ExcelReportCell> table);

        /// <summary>
        /// Writes Excel report to stream.
        /// </summary>
        /// <param name="table">Excel report table.</param>
        /// <param name="stream">Stream to write Excel report to.</param>
        void WriteToStream(IReportTable<ExcelReportCell> table, Stream stream);
    }
}
