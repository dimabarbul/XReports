using System.Collections.Generic;
using System.Linq;
using Reports.Core.Interfaces;

namespace Reports.Core.Models
{
    public class VerticalReportSchema<TSourceEntity> : ReportSchema<TSourceEntity>
    {
        public ComplexHeader[] ComplexHeaders { get; protected internal set; }
        public Dictionary<string, ReportCellProperty[]> ComplexHeaderProperties { get; protected internal set; }

        public override IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source)
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>();

            this.BuildHeader(table);
            this.BuildBody(table, source);

            return table;
        }

        private void BuildBody(ReportTable<ReportCell> table, IEnumerable<TSourceEntity> source)
        {
            table.Rows = this.GetRows(source);
        }

        private IEnumerable<IEnumerable<ReportCell>> GetRows(IEnumerable<TSourceEntity> source)
        {
            return source
                .Select(entity => this.CellsProviders
                    .Select(p => this.AddTableProperties(p.CreateCell(entity)))
                );
        }


        private void BuildHeader(ReportTable<ReportCell> table)
        {
            SpannableHeader[,] header = this.BuildHeaderCells();

            this.MoveSpanHeaderTitleUp(header);

            table.HeaderRows = this.GetHeaderCells(header);
        }

        private SpannableHeader[,] BuildHeaderCells()
        {
            int complexHeaderRowsCount =
                this.ComplexHeaders.Any() ?
                this.ComplexHeaders.Max(h => h.RowIndex) + 1 :
                0;
            SpannableHeader[,] headerCells = new SpannableHeader[complexHeaderRowsCount + 1, this.CellsProviders.Length];

            this.AddComplexHeaderCells(headerCells);
            this.AddRegularHeaderCells(headerCells);

            return headerCells;
        }

        private void AddComplexHeaderCells(SpannableHeader[,] headerCells)
        {
            foreach (ComplexHeader header in this.ComplexHeaders
                .OrderBy(h => h.RowIndex)
                .ThenBy(h => h.StartIndex))
            {
                headerCells[header.RowIndex, header.StartIndex] = new SpannableHeader(this.CreateComplexHeaderCell(header.Title, header.EndIndex - header.StartIndex + 1));

                for (int i = header.StartIndex + 1; i <= header.EndIndex; i++)
                {
                    headerCells[header.RowIndex, i] = SpannableHeader.Spanned;
                }
            }
        }

        private ReportCell CreateComplexHeaderCell(string title, int columnSpan)
        {
            ReportCell cell = new ReportCell<string>(title)
            {
                ColumnSpan = columnSpan,
            };

            if (this.ComplexHeaderProperties.ContainsKey(title))
            {
                foreach (ReportCellProperty property in this.ComplexHeaderProperties[title])
                {
                    cell.AddProperty(property);
                }
            }

            return cell;
        }

        private void AddRegularHeaderCells(SpannableHeader[,] headerCells)
        {
            int lastHeaderRowIndex = headerCells.GetLength(0) - 1;
            for (int i = 0; i < this.CellsProviders.Length; i++)
            {
                headerCells[lastHeaderRowIndex, i] = new SpannableHeader(this.CellsProviders[i].CreateHeaderCell());
            }
        }

        private void MoveSpanHeaderTitleUp(SpannableHeader[,] header)
        {
            int rowsCount = header.GetLength(0);
            int columnsCount = header.GetLength(1);
            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    if (header[rowIndex, columnIndex] != null)
                    {
                        continue;
                    }

                    this.MoveTitleUpForEmptyCell(header, rowIndex, columnIndex);
                }
            }
        }

        private void MoveTitleUpForEmptyCell(SpannableHeader[,] header, int rowIndex, int columnIndex)
        {
            int? filledRowIndex = this.GetNextFilledRowIndex(header, rowIndex, columnIndex);
            if (filledRowIndex == null)
            {
                return;
            }

            header[rowIndex, columnIndex] = header[(int) filledRowIndex, columnIndex];
            header[rowIndex, columnIndex].Cell.RowSpan = (int) filledRowIndex - rowIndex + 1;

            for (int i = rowIndex + 1; i <= filledRowIndex; i++)
            {
                header[i, columnIndex] = SpannableHeader.Spanned;
            }
        }

        private int? GetNextFilledRowIndex(SpannableHeader[,] header, int rowIndex, int columnIndex)
        {
            int rowsCount = header.GetLength(0);
            for (int currentRowIndex = rowIndex + 1; currentRowIndex < rowsCount; currentRowIndex++)
            {
                if (header[currentRowIndex, columnIndex] != null && !header[currentRowIndex, columnIndex].IsSpanned)
                {
                    return currentRowIndex;
                }
            }

            return null;
        }

        private IEnumerable<IEnumerable<ReportCell>> GetHeaderCells(SpannableHeader[,] header)
        {
            int rowsCount = header.GetLength(0);
            int columnsCount = header.GetLength(1);
            ReportCell[] row = new ReportCell[columnsCount];

            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    row[columnIndex] = header[rowIndex, columnIndex].Cell;
                }

                yield return row;
            }

            // int rowsCount = header.GetLength(0);
            // int columnsCount = header.GetLength(1);
            // ReportCell[] row = new ReportCell[columnsCount];
            //
            // for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            // {
            //     for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
            //     {
            //         row[columnIndex] = null;
            //         if (header[rowIndex, columnIndex].IsSpanned || header[rowIndex, columnIndex].IsUsed)
            //         {
            //             continue;
            //         }
            //
            //         row[columnIndex] = this.CreateHeaderCell(header, rowIndex, columnIndex);
            //     }
            //
            //     yield return row;
            // }
        }

        // private ReportCell<string> CreateHeaderCell(SpannableHeader[,] header, int rowIndex, int columnIndex)
        // {
        //     int rowSpan = this.CalculateRowSpan(header, rowIndex, columnIndex);
        //     int columnSpan = this.CalculateColumnSpan(header, rowIndex, columnIndex);
        //     if (rowSpan > 1)
        //     {
        //         columnSpan = 1;
        //     }
        //
        //     for (int i = 1; i < rowSpan; i++)
        //     {
        //         header[rowIndex + i, columnIndex].MarkUsed();
        //     }
        //
        //     for (int i = 1; i < columnSpan; i++)
        //     {
        //         header[rowIndex, columnIndex + i].MarkUsed();
        //     }
        //
        //     string title = header[rowIndex, columnIndex].Title;
        //     ReportCell<string> headerCell = new ReportCell<string>(title)
        //     {
        //         RowSpan = rowSpan,
        //         ColumnSpan = columnSpan
        //     };
        //
        //     if (this.headerCellProperties.ContainsKey(title))
        //     {
        //         headerCell.Properties.AddRange(this.headerCellProperties[title]);
        //     }
        //
        //     foreach (IReportCellProcessor<TSourceEntity> processor in this.headerCellProcessors)
        //     {
        //         processor.Process(headerCell, default);
        //     }
        //
        //     return headerCell;
        // }

        // private int CalculateRowSpan(SpannableHeader[,] header, int rowIndex, int columnIndex)
        // {
        //     int rowSpan = 1;
        //     for (int i = rowIndex + 1; i < header.GetLength(0); i++)
        //     {
        //         if (!header[i, columnIndex].IsSpanned || header[i, columnIndex].IsUsed)
        //         {
        //             break;
        //         }
        //
        //         rowSpan++;
        //     }
        //
        //     return rowSpan;
        // }
        //
        // private int CalculateColumnSpan(SpannableHeader[,] header, int rowIndex, int columnIndex)
        // {
        //     int columnSpan = 1;
        //     for (int i = columnIndex + 1; i < header.GetLength(1); i++)
        //     {
        //         if (!header[rowIndex, i].IsSpanned || header[rowIndex, i].IsUsed)
        //         {
        //             break;
        //         }
        //
        //         columnSpan++;
        //     }
        //
        //     return columnSpan;
        // }

        private class SpannableHeader
        {
            public static readonly SpannableHeader Spanned = new SpannableHeader(null) { IsSpanned = true };

            public SpannableHeader(ReportCell cell)
            {
                this.Cell = cell;
            }

            public ReportCell Cell { get; set; }
            public bool IsSpanned { get; private set; }
            // public bool IsUsed { get; private set; }

            // public void MarkUsed()
            // {
            //     this.IsUsed = true;
            // }
        }
    }
}
