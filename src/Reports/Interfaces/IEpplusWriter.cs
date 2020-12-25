using System.IO;
using Reports.Models;

namespace Reports.Interfaces
{
    public interface IEpplusWriter
    {
        IEpplusWriter AddFormatter(IEpplusFormatter formatter);
        void WriteToFile(IReportTable<ExcelReportCell> table, string fileName);
        Stream WriteToStream(IReportTable<ExcelReportCell> table);
    }
}
