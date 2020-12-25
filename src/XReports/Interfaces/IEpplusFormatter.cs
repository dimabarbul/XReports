using OfficeOpenXml;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IEpplusFormatter
    {
        void Format(ExcelRange worksheetCell, ExcelReportCell cell);
    }
}
