using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using XReports.Enums;
using XReports.Models;
using XReports.Interfaces;
using XReports.Properties;

namespace XReports.Writers
{
    public class EpplusWriter : IEpplusWriter
    {
        private const string WorksheetName = "Data";

        private readonly Dictionary<int, ExcelReportCell> columnFormatCells = new Dictionary<int, ExcelReportCell>();
        private readonly List<IEpplusFormatter> formatters = new List<IEpplusFormatter>();

        public IEpplusWriter AddFormatter(IEpplusFormatter formatter)
        {
            this.formatters.Add(formatter);

            return this;
        }

        public void WriteToFile(IReportTable<ExcelReportCell> table, string fileName)
        {
            using ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName));

            this.WriteReport(table, excelPackage);

            excelPackage.Save();
        }

        public Stream WriteToStream(IReportTable<ExcelReportCell> table)
        {
            Stream stream = new MemoryStream();
            using ExcelPackage excelPackage = new ExcelPackage(stream);

            this.WriteReport(table, excelPackage);

            excelPackage.Save();

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        protected virtual ExcelAddress WriteHeader(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table, int startRow, int startColumn)
        {
            int maxColumn = startColumn;
            int row = startRow;

            foreach (IEnumerable<ExcelReportCell> headerRow in table.HeaderRows)
            {
                int column = startColumn;
                foreach (ExcelReportCell cell in headerRow)
                {
                    if (cell != null)
                    {
                        this.WriteHeaderCell(worksheet.Cells[row, column], cell);
                    }
                    column++;
                }

                if (column > maxColumn)
                {
                    maxColumn = column;
                }

                row++;
            }

            row--;
            maxColumn--;

            if (row < startRow)
            {
                return null;
            }

            ExcelAddress address = new ExcelAddress(startRow, startColumn, row, maxColumn);
            this.FormatHeader(worksheet, address);

            return address;
        }

        protected virtual void FormatHeader(ExcelWorksheet worksheet, ExcelAddress headerAddress)
        {
            ExcelRange range = worksheet.Cells[headerAddress.Address];

            range.Style.Font.Bold = true;
        }

        protected virtual ExcelAddress WriteBody(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table, int startRow, int startColumn)
        {
            int maxColumn = startColumn;
            int row = startRow;

            foreach (IEnumerable<ExcelReportCell> bodyRow in table.Rows)
            {
                int column = startColumn;
                foreach (ExcelReportCell cell in bodyRow)
                {
                    if (cell != null)
                    {
                        this.WriteCell(worksheet.Cells[row, column], cell);
                    }
                    column++;
                }

                if (column > maxColumn)
                {
                    maxColumn = column;
                }

                row++;
            }

            row--;
            maxColumn--;

            this.ApplyColumnFormat(worksheet, startRow, row);

            return row >= startRow ?
                new ExcelAddress(startRow, startColumn, row, maxColumn) :
                null;
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
            worksheetCell.Value = cell.Value;

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

            if (cell.HasProperty<SameColumnFormatProperty>())
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

        protected ExcelAddress WriteReportToWorksheet(IReportTable<ExcelReportCell> table, ExcelWorksheet worksheet, int row, int column)
        {
            ExcelAddress headerAddress = this.WriteHeader(worksheet, table, row, column);
            ExcelAddress bodyAddress = this.WriteBody(
                worksheet, table, headerAddress == null ? row : (headerAddress.End.Row + 1), column);

            this.PostCreate(worksheet, headerAddress, bodyAddress);

            if (headerAddress == null && bodyAddress == null)
            {
                return null;
            }

            return new ExcelAddress(
                (headerAddress ?? bodyAddress).Start.Row,
                (headerAddress ?? bodyAddress).Start.Column,
                (bodyAddress ?? headerAddress).End.Row,
                (bodyAddress ?? headerAddress).End.Column
            );
        }

        private void WriteReport(IReportTable<ExcelReportCell> table, ExcelPackage excelPackage)
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(WorksheetName);

            this.WriteReportToWorksheet(table, worksheet, 1, 1);
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
