using OfficeOpenXml;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Excel.EpplusWriter;

namespace ConsoleApp1
{
    public class MyExcelWriter : EpplusWriter
    {
        protected override ExcelAddress WriteBody(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
        {
            ExcelAddress bodyAddress = base.WriteBody(worksheet, table);

            worksheet.Cells[1, 1, bodyAddress.End.Row > 100 ? 100 : bodyAddress.End.Row, bodyAddress.End.Column].AutoFitColumns();

            return bodyAddress;
        }
    }
}
