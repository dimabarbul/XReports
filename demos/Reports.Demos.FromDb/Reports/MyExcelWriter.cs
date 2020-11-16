using System;
using OfficeOpenXml;
using Reports.Excel.EpplusWriter;

namespace Reports.Demos.FromDb.Reports
{
    public class MyExcelWriter : EpplusWriter
    {
        protected override void PostCreate(ExcelWorksheet worksheet, ExcelAddress headerAddress, ExcelAddress bodyAddress)
        {
            base.PostCreate(worksheet, headerAddress, bodyAddress);

            worksheet.Cells[headerAddress.Start.Row, headerAddress.Start.Column, Math.Min(100, bodyAddress.End.Row), bodyAddress.End.Column].AutoFitColumns();
        }
    }
}
