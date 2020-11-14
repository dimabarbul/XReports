using OfficeOpenXml;
using Reports.Excel.EpplusWriter;
using Reports.Excel.Models;
using Reports.Interfaces;

namespace ConsoleApp1
{
    public class MyExcelWriter : EpplusWriter
    {
        protected override void FormatCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            base.FormatCell(worksheetCell, cell);

            if (cell.HasProperty<MyCustomFormatProperty>())
            {
                worksheetCell.Style.Numberformat.Format = $"[>=90]0;[<90]0.0";
            }
        }

        protected override ExcelAddress WriteBody(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
        {
            ExcelAddress bodyAddress = base.WriteBody(worksheet, table);

            worksheet.Cells[1, 1, bodyAddress.End.Row > 100 ? 100 : bodyAddress.End.Row, bodyAddress.End.Column].AutoFitColumns();

            return bodyAddress;
        }
    }
}
