using OfficeOpenXml;

namespace XReports.Excel.Writers
{
    public interface IEpplusFormatter
    {
        void Format(ExcelRange worksheetCell, ExcelReportCell cell);
    }
}
