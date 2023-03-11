using System.IO;
using XReports.Models;
using XReports.Table;

namespace XReports.Interfaces
{
    public interface IEpplusWriter
    {
        void WriteToFile(IReportTable<ExcelReportCell> table, string fileName);

        Stream WriteToStream(IReportTable<ExcelReportCell> table);

        void WriteToStream(IReportTable<ExcelReportCell> table, Stream stream);
    }
}
