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
    /// <summary>
    /// Writer of Excel report using EPPlus library.
    /// </summary>
    /// <remarks>The class is not thread-safe, i.e., do not use one instance of the writer to write multiple reports in parallel.</remarks>
    public class EpplusWriter : IEpplusWriter
    {
        private readonly Dictionary<int, ExcelReportCell> columnFormatCells = new Dictionary<int, ExcelReportCell>();
        private readonly IEpplusFormatter[] formatters;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpplusWriter"/> class. The writer will use no formatters.
        /// </summary>
        public EpplusWriter()
            : this(Enumerable.Empty<IEpplusFormatter>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EpplusWriter"/> class.
        /// </summary>
        /// <param name="formatters">Formatters to use.</param>
        public EpplusWriter(IEnumerable<IEpplusFormatter> formatters)
        {
            this.formatters = formatters.ToArray();
        }

        /// <summary>
        /// Gets or sets name of Excel worksheet to create and write to.
        /// </summary>
        protected string WorksheetName { get; set; } = "Data";

        /// <summary>
        /// Gets or sets 1-based row number to start writing report at.
        /// </summary>
        protected int StartRow { get; set; } = 1;

        /// <summary>
        /// Gets or sets 1-based column number to start writing report at.
        /// </summary>
        protected int StartColumn { get; set; } = 1;

        /// <inheritdoc />
        public void WriteToFile(IReportTable<ExcelReportCell> table, string fileName)
        {
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName)))
            {
                this.WriteReport(table, excelPackage);

                excelPackage.Save();
            }
        }

        /// <inheritdoc />
        public Stream WriteToStream(IReportTable<ExcelReportCell> table)
        {
            Stream stream = new MemoryStream();

            this.WriteToStream(table, stream);

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        /// <inheritdoc />
        public void WriteToStream(IReportTable<ExcelReportCell> table, Stream stream)
        {
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                this.WriteReport(table, excelPackage);

                excelPackage.Save();
            }
        }

        /// <summary>
        /// Writes report header.
        /// </summary>
        /// <param name="worksheet">Excel worksheet to write to.</param>
        /// <param name="table">Report table to write.</param>
        /// <param name="startRow">1-based row number to start writing at.</param>
        /// <param name="startColumn">1-based column number to start writing at.</param>
        /// <returns>Excel address of report header.</returns>
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

        /// <summary>
        /// Formats report header.
        /// </summary>
        /// <param name="worksheet">Excel worksheet where report header is on.</param>
        /// <param name="headerAddress">Excel address of report header.</param>
        protected virtual void FormatHeader(ExcelWorksheet worksheet, ExcelAddress headerAddress)
        {
            ExcelRange range = worksheet.Cells[headerAddress.Address];

            range.Style.Font.Bold = true;
        }

        /// <summary>
        /// Writes report body.
        /// </summary>
        /// <param name="worksheet">Excel worksheet to write to.</param>
        /// <param name="table">Report table to write.</param>
        /// <param name="startRow">1-based row number to start writing at.</param>
        /// <param name="startColumn">1-based column number to start writing at.</param>
        /// <returns>Excel address of report body.</returns>
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

        /// <summary>
        /// Applies column format for each column that have same column format.
        /// </summary>
        /// <param name="worksheet">Excel worksheet to write to.</param>
        /// <param name="startRow">1-based row number where report body starts.</param>
        /// <param name="endRow">1-based row number where report body ends.</param>
        protected virtual void ApplyColumnFormat(ExcelWorksheet worksheet, int startRow, int endRow)
        {
            foreach (KeyValuePair<int, ExcelReportCell> columnFormatCell in this.columnFormatCells)
            {
                this.FormatCell(worksheet.Cells[startRow, columnFormatCell.Key, endRow, columnFormatCell.Key], columnFormatCell.Value);
            }
        }

        /// <summary>
        /// Writes report header cell.
        /// </summary>
        /// <param name="worksheet">Excel worksheet to write to.</param>
        /// <param name="row">1-based row number to write cell at.</param>
        /// <param name="col">1-based column number to write cell at.</param>
        /// <param name="cell">Report cell to write.</param>
        protected virtual void WriteHeaderCell(ExcelWorksheet worksheet, int row, int col, ExcelReportCell cell)
        {
            this.WriteCell(worksheet, row, col, cell);
        }

        /// <summary>
        /// Writes report body cell.
        /// </summary>
        /// <param name="worksheet">Excel worksheet to write to.</param>
        /// <param name="row">1-based row number to write cell at.</param>
        /// <param name="col">1-based column number to write cell at.</param>
        /// <param name="cell">Report cell to write.</param>
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

        /// <summary>
        /// Formats single Excel cell.
        /// </summary>
        /// <param name="worksheet">Excel worksheet to write to.</param>
        /// <param name="row">1-based row number of the cell to format.</param>
        /// <param name="col">1-based column number of the cell to format.</param>
        /// <param name="cell">Report cell to take format from.</param>
        protected void FormatCell(ExcelWorksheet worksheet, int row, int col, ExcelReportCell cell)
        {
            this.FormatCell(worksheet.Cells[row, col], cell);
        }

        /// <summary>
        /// Formats Excel cell range.
        /// </summary>
        /// <param name="excelRange">Excel cell range to format.</param>
        /// <param name="cell">Report cell to take format from.</param>
        protected virtual void FormatCell(ExcelRange excelRange, ExcelReportCell cell)
        {
            if (cell.HorizontalAlignment != null)
            {
                excelRange.Style.HorizontalAlignment = GetAlignment(cell.HorizontalAlignment.Value);
            }

            if (!string.IsNullOrEmpty(cell.NumberFormat))
            {
                excelRange.Style.Numberformat.Format = cell.NumberFormat;
            }

            if (cell.IsBold)
            {
                excelRange.Style.Font.Bold = true;
            }

            if (cell.FontColor != null)
            {
                excelRange.Style.Font.Color.SetColor(cell.FontColor.Value);
            }

            if (cell.BackgroundColor != null)
            {
                excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                excelRange.Style.Fill.BackgroundColor.SetColor(cell.BackgroundColor.Value);
            }

            foreach (IEpplusFormatter formatter in this.formatters)
            {
                formatter.Format(excelRange, cell);
            }
        }

        /// <summary>
        /// Executes post-creation actions. The method is executed after the
        /// report has been fully written.
        /// </summary>
        /// <param name="worksheet">Excel worksheet to write to.</param>
        /// <param name="headerAddress">Excel address of report header.</param>
        /// <param name="bodyAddress">Excel address of report body.</param>
        protected virtual void PostCreate(
            ExcelWorksheet worksheet,
            ExcelAddress headerAddress,
            ExcelAddress bodyAddress)
        {
        }

        /// <summary>
        /// Writes report to Excel worksheet.
        /// </summary>
        /// <param name="table">Report table to write.</param>
        /// <param name="worksheet">Excel worksheet to write to.</param>
        /// <param name="row">1-based row number to start writing at.</param>
        /// <param name="column">1-based column number to start writing at.</param>
        /// <returns>Excel address of report.</returns>
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

        private void WriteReport(IReportTable<ExcelReportCell> table, ExcelPackage excelPackage)
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(this.WorksheetName);
            this.columnFormatCells.Clear();

            this.WriteReportToWorksheet(table, worksheet, this.StartRow, this.StartColumn);
        }
    }
}
