using OfficeOpenXml;
using OfficeOpenXml.Style;
using Reports.Interfaces;
using Reports.Models;
using Reports.Writers;

namespace ConsoleApp1
{
    public class MyExcelWriter : EpplusWriter
    {
        protected override void FormatCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            base.FormatCell(worksheetCell, cell);

            if (cell is MyExcelReportCell myExcelReportCell)
            {
                if (myExcelReportCell.BorderColor.HasValue)
                {
                    worksheetCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, myExcelReportCell.BorderColor.Value);
                }
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
