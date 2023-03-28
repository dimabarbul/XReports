using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using XReports.ReportCellProperties;
using XReports.Table;

namespace XReports.Excel.Writers
{
    public class EpplusWriter : IEpplusWriter
    {
        private readonly Dictionary<int, ExcelReportCell> columnFormatCells = new Dictionary<int, ExcelReportCell>();
        private readonly IEpplusFormatter[] formatters;

        public EpplusWriter()
            : this(Enumerable.Empty<IEpplusFormatter>())
        {
        }

        public EpplusWriter(IEnumerable<IEpplusFormatter> formatters)
        {
            this.formatters = formatters.ToArray();
        }

        protected string WorksheetName { get; set; } = "Data";

        protected int StartRow { get; set; } = 1;

        protected int StartColumn { get; set; } = 1;

        public void WriteToFile(IReportTable<ExcelReportCell> table, string fileName)
        {
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName)))
            {
                this.WriteReport(table, excelPackage);

                excelPackage.Save();
            }
        }

        public Stream WriteToStream(IReportTable<ExcelReportCell> table)
        {
            Stream stream = new MemoryStream();

            this.WriteToStream(table, stream);

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public void WriteToStream(IReportTable<ExcelReportCell> table, Stream stream)
        {
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                this.WriteReport(table, excelPackage);

                excelPackage.Save();
            }
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
                        this.WriteHeaderCell(worksheet, row, column, cell);
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
                        this.WriteCell(worksheet, row, column, cell);
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
            foreach (KeyValuePair<int, ExcelReportCell> columnFormatCell in this.columnFormatCells)
            {
                this.FormatCell(worksheet.Cells[startRow, columnFormatCell.Key, endRow, columnFormatCell.Key], columnFormatCell.Value);
            }
        }

        protected virtual void WriteHeaderCell(ExcelWorksheet worksheet, int row, int col, ExcelReportCell cell)
        {
            this.WriteCell(worksheet, row, col, cell);
        }

        protected virtual void WriteCell(ExcelWorksheet worksheet, int row, int col, ExcelReportCell cell)
        {
            worksheet.SetValue(row, col, cell.GetUnderlyingValue());

            if (cell.ColumnSpan > 1 || cell.RowSpan > 1)
            {
                ExcelRange excelRange = worksheet
                    .Cells[row, col, row + cell.RowSpan - 1, col + cell.ColumnSpan - 1];
                excelRange.Merge = true;

                if (cell.RowSpan > 1)
                {
                    excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
            }

            if (cell.HasProperty<SameColumnFormatProperty>())
            {
                if (!this.columnFormatCells.ContainsKey(col))
                {
                    this.columnFormatCells.Add(col, (ExcelReportCell)cell.Clone());
                }

                return;
            }

            this.FormatCell(worksheet, row, col, cell);
        }

        protected void FormatCell(ExcelWorksheet worksheet, int row, int col, ExcelReportCell cell)
        {
            this.FormatCell(worksheet.Cells[row, col], cell);
        }

        protected virtual void FormatCell(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            if (cell.HorizontalAlignment != null)
            {
                worksheetCell.Style.HorizontalAlignment = GetAlignment(cell.HorizontalAlignment.Value);
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

        protected virtual void PostCreate(
            ExcelWorksheet worksheet,
            ExcelAddress headerAddress,
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
                (bodyAddress ?? headerAddress).End.Column);
        }

        private void WriteReport(IReportTable<ExcelReportCell> table, ExcelPackage excelPackage)
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(this.WorksheetName);
            this.columnFormatCells.Clear();

            this.WriteReportToWorksheet(table, worksheet, this.StartRow, this.StartColumn);
        }

        private static ExcelHorizontalAlignment GetAlignment(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Center:
                    return ExcelHorizontalAlignment.Center;
                case Alignment.Left:
                    return ExcelHorizontalAlignment.Left;
                case Alignment.Right:
                    return ExcelHorizontalAlignment.Right;
                default:
                    throw new ArgumentOutOfRangeException(nameof(alignment));
            }
        }
    }
}
