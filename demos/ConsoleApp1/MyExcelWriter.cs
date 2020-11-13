using OfficeOpenXml;
using Reports.Excel.EpplusWriter;
using Reports.Excel.Models;

namespace ConsoleApp1
{
    public class MyExcelWriter : EpplusWriter
    {
        protected override void WriteCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            base.WriteCell(worksheetCell, cell);

            if (cell.HasProperty<MyCustomFormatProperty>())
            {
                worksheetCell.Style.Numberformat.Format = $"[>=90]0;[<90]0.0";
            }
        }
    }
}
