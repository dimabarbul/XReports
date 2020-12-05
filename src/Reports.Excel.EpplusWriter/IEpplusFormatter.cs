using OfficeOpenXml;
using Reports.Core.Models;

namespace Reports.Excel.EpplusWriter
{
    public interface IEpplusFormatter
    {
        void Format(ExcelRange worksheetCell, ExcelReportCell cell);
    }
}
