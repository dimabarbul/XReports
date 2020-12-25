using OfficeOpenXml;
using Reports.Models;

namespace Reports.Interfaces
{
    public interface IEpplusFormatter
    {
        void Format(ExcelRange worksheetCell, ExcelReportCell cell);
    }
}
