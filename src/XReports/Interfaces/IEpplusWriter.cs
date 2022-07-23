using System.IO;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IEpplusWriter
    {
        IEpplusWriter AddFormatter(IEpplusFormatter formatter);

        void WriteToFile(IReportTable<ExcelReportCell> table, string fileName);

        Stream WriteToStream(IReportTable<ExcelReportCell> table);

        void WriteToStream(IReportTable<ExcelReportCell> table, Stream stream);
    }
}
