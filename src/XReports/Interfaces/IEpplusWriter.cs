using System.IO;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IEpplusWriter
    {
        void WriteToFile(IReportTable<ExcelReportCell> table, string fileName);

        Stream WriteToStream(IReportTable<ExcelReportCell> table);

        void WriteToStream(IReportTable<ExcelReportCell> table, Stream stream);
    }
}
