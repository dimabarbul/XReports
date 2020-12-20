using System.IO;
using Reports.Core.Interfaces;
using Reports.Core.Models;

namespace Reports.Excel.EpplusWriter
{
    public interface IEpplusWriter
    {
        IEpplusWriter AddFormatter(IEpplusFormatter formatter);
        void WriteToFile(IReportTable<ExcelReportCell> table, string fileName);
        Stream WriteToStream(IReportTable<ExcelReportCell> table);
    }
}
