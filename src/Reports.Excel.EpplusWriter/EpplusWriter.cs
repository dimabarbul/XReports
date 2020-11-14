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

        protected virtual void WriteHeader(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
        {
            int startRow = this.row;

            int maxColumn = 1;
            foreach (IEnumerable<ExcelReportCell> headerRow in table.HeaderRows)
            {
                int column = 1;
                foreach (ExcelReportCell cell in headerRow)
                {
                    if (cell != null)
                    {
                        this.WriteHeaderCell(worksheet.Cells[this.row, column], cell);
                    }
                    column++;
                }

                maxColumn = Math.Max(maxColumn, column);

                this.row++;
            }

            if (this.row > startRow)
            {
                worksheet.Cells[startRow, 1, this.row - 1, maxColumn].Style.Font.Bold = true;
                worksheet.Cells[startRow, 1, this.row - 1, maxColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }

        protected virtual void WriteBody(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
        {
            foreach (IEnumerable<ExcelReportCell> bodyRow in table.Rows)
            {
                int column = 1;
                foreach (ExcelReportCell cell in bodyRow)
                {
                    if (cell != null)
                    {
                        this.WriteCell(worksheet.Cells[this.row, column], cell);
                    }
                    column++;
                }

                this.row++;
            }
        }

        protected virtual void WriteHeaderCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            this.WriteCell(worksheetCell, cell);
        }

        protected virtual void WriteCell(ExcelRange worksheetCell, ExcelReportCell cell)
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

            if (cell.ColumnSpan > 1 || cell.RowSpan > 1)
            {
                int startRow = worksheetCell.Start.Row;
                int startColumn = worksheetCell.Start.Column;
                worksheetCell.Worksheet
                    .Cells[startRow, startColumn, startRow + cell.RowSpan - 1, startColumn + cell.ColumnSpan - 1]
                    .Merge = true;
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
