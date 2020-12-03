using OfficeOpenXml;
using Reports.Models;

namespace Reports.Excel.EpplusWriter
{
    public interface IEpplusFormatter
    {
        void Format(ExcelRange worksheetCell, ExcelReportCell cell);
    }
}
