using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Reports.Enums;
using Reports.Excel.Models;
using Reports.Interfaces;

namespace Reports.Excel.EpplusWriter
{
    public class EpplusWriter
    {
        private int row;

        public void WriteToFile(IReportTable<ExcelReportCell> table, string fileName)
        {
            using ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Data");

            this.row = 1;
            this.WriteHeader(worksheet, table);
            this.WriteBody(worksheet, table);

            excelPackage.Save();
        }

        private void WriteHeader(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
        {
            int startRow = this.row;

            int col = 1;
            foreach (IEnumerable<ExcelReportCell> headerRow in table.HeaderRows)
            {
                foreach (ExcelReportCell cell in headerRow)
                {
                    this.WriteHeaderCell(worksheet.Cells[this.row, col], cell);
                    col++;
                }

                this.row++;
            }

            if (this.row > startRow)
            {
                worksheet.Cells[startRow, 1, this.row - 1, col].Style.Font.Bold = true;
                worksheet.Cells[startRow, 1, this.row - 1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }

        private void WriteBody(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
        {
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
        }

        private void WriteHeaderCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            this.WriteCell(worksheetCell, cell);
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

            if (cell.IsBold)
            {
                worksheetCell.Style.Font.Bold = true;
            }

            if (cell.FontColor != null)
            {
                worksheetCell.Style.Font.Color.SetColor(cell.FontColor.Value);
            }

            if (cell.BackgroundColor != null)
            {
                worksheetCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheetCell.Style.Fill.BackgroundColor.SetColor(cell.BackgroundColor.Value);
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
