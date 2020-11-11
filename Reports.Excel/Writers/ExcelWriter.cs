using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Reports.Enums;
using Reports.Excel.Models;
using Reports.Interfaces;

namespace Reports.Excel.Writers
{
    public class ExcelWriter
    {
        private int row;

        public void WriteToFile(IReportTable<ExcelReportCell> table, string fileName)
        {
            using ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Data");

            this.row = 2;
            worksheet.SetValue(1, 1, 1);
            worksheet.SetValue(1, 2, "1");
            worksheet.SetValue(1, 3, 1.00);
            worksheet.Cells[1, 1, 1, 3].Style.Numberformat.Format = "#.00";
            // worksheet.Cells[1, 1].Style.Numberformat.Format = "#.00";
            // worksheet.Cells[1, 1].Style.Numberformat.Format = "#.00";
            this.WriteHeader(worksheet, table);
            this.WriteBody(worksheet, table);

            excelPackage.Save();
        }

        private void WriteHeader(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
        {
            foreach (IEnumerable<ExcelReportCell> headerRow in table.HeaderRows)
            {
                int col = 1;
                foreach (ExcelReportCell cell in headerRow)
                {
                    // worksheet.SetValue(this.row, col, cell.Value);
                    this.WriteHeaderCell(worksheet.Cells[this.row, col], cell);
                    col++;
                }

                this.row++;
            }
        }

        private void WriteBody(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
        {
            int startRow = this.row;

            int col = 1;
            foreach (IEnumerable<ExcelReportCell> bodyRow in table.Rows)
            {
                col = 1;
                foreach (ExcelReportCell cell in bodyRow)
                {
                    this.WriteCell(worksheet.Cells[this.row, col], cell);
                    col++;
                }

                this.row++;
            }

            // for (int i = 1; i <= col; i++)
            // {
            //     worksheet.Cells[startRow, i, this.row - 1, i].Style.HorizontalAlignment = this.GetAlignment(AlignmentType.Center);
            // }
        }

        private void WriteHeaderCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            this.WriteCell(worksheetCell, cell);
            worksheetCell.Style.Font.Bold = true;
        }

        private void WriteCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            worksheetCell.Value = cell.InternalValue;
            if (cell.AlignmentType != null)
            {
                worksheetCell.Style.HorizontalAlignment = this.GetAlignment(cell.AlignmentType.Value);
            }

            if (!string.IsNullOrEmpty(cell.NumberFormat))
            {
                worksheetCell.Style.Numberformat.Format = cell.NumberFormat;
            }
        }

        private ExcelHorizontalAlignment GetAlignment(AlignmentType alignmentType)
        {
            return alignmentType switch
            {
                AlignmentType.Center => ExcelHorizontalAlignment.Center,
                AlignmentType.Left => ExcelHorizontalAlignment.Left,
                AlignmentType.Right => ExcelHorizontalAlignment.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(alignmentType)),
            };
        }
    }
}
