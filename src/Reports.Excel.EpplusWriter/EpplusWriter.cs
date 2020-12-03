using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Reports.Enums;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Excel.EpplusWriter
{
    public class EpplusWriter
    {
        private readonly Dictionary<int, ExcelReportCell> columnFormatCells = new Dictionary<int, ExcelReportCell>();
        private readonly List<IEpplusFormatter> formatters = new List<IEpplusFormatter>();

        private int row;

        public EpplusWriter AddFormatter(IEpplusFormatter formatter)
        {
            this.formatters.Add(formatter);

            return this;
        }

        public void WriteToFile(IReportTable<ExcelReportCell> table, string fileName)
        {
            using ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName));

            this.WriteReport(table, excelPackage);
        }

        public Stream WriteToStream(IReportTable<ExcelReportCell> table)
        {
            Stream stream = new MemoryStream();
            using ExcelPackage excelPackage = new ExcelPackage(stream);

            this.WriteReport(table, excelPackage);

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        protected virtual ExcelAddress WriteHeader(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
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

                if (column > maxColumn)
                {
                    maxColumn = column;
                }

                this.row++;
            }

            if (this.row == startRow)
            {
                return null;
            }

            ExcelAddress address = new ExcelAddress(startRow, 1, this.row - 1, maxColumn - 1);
            this.FormatHeader(worksheet, address);

            return address;
        }

        protected virtual void FormatHeader(ExcelWorksheet worksheet, ExcelAddress headerAddress)
        {
            ExcelRange range = worksheet.Cells[headerAddress.Address];

            range.Style.Font.Bold = true;
        }

        protected virtual ExcelAddress WriteBody(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
        {
            int startRow = this.row;
            int maxColumn = 1;

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

                if (column > maxColumn)
                {
                    maxColumn = column;
                }

                this.row++;
            }

            this.ApplyColumnFormat(worksheet, startRow, this.row - 1);

            return new ExcelAddress(startRow, 1, this.row - 1, maxColumn - 1);
        }

        protected virtual void ApplyColumnFormat(ExcelWorksheet worksheet, int startRow, int endRow)
        {
            foreach ((int column, ExcelReportCell cell) in this.columnFormatCells)
            {
                this.FormatCell(worksheet.Cells[startRow, column, endRow, column], cell);
            }
        }

        protected virtual void WriteHeaderCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            this.WriteCell(worksheetCell, cell);
        }

        protected virtual void WriteCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            worksheetCell.Value = cell.InternalValue;

            if (cell.ColumnSpan > 1 || cell.RowSpan > 1)
            {
                int startRow = worksheetCell.Start.Row;
                int startColumn = worksheetCell.Start.Column;
                ExcelRange excelRange = worksheetCell.Worksheet
                    .Cells[startRow, startColumn, startRow + cell.RowSpan - 1, startColumn + cell.ColumnSpan - 1];
                excelRange.Merge = true;

                if (cell.RowSpan > 1)
                {
                    excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
            }

            if (cell.HasProperty<ColumnSameFormatProperty>())
            {
                int column = worksheetCell.Start.Column;
                if (!this.columnFormatCells.ContainsKey(column))
                {
                    this.columnFormatCells.Add(column, cell);
                }

                return;
            }

            this.FormatCell(worksheetCell, cell);
        }

        protected virtual void FormatCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
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

            foreach (IEpplusFormatter formatter in this.formatters)
            {
                formatter.Format(worksheetCell, cell);
            }
        }

        protected virtual void PostCreate(ExcelWorksheet worksheet, ExcelAddress headerAddress,
            ExcelAddress bodyAddress)
        {
        }

        private void WriteReport(IReportTable<ExcelReportCell> table, ExcelPackage excelPackage)
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Data");

            this.row = 1;
            ExcelAddress headerAddress = this.WriteHeader(worksheet, table);
            ExcelAddress bodyAddress = this.WriteBody(worksheet, table);

            this.PostCreate(worksheet, headerAddress, bodyAddress);

            excelPackage.Save();
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
