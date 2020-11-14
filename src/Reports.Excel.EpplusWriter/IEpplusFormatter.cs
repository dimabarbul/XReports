using OfficeOpenXml;
using Reports.Excel.Models;

namespace Reports.Excel.EpplusWriter
{
    public interface IEpplusFormatter
    {
        void Format(ExcelRange worksheetCell, ExcelReportCell cell);
    }
}
