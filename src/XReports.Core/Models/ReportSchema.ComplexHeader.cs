using System.Collections.Generic;
using System.Linq;

namespace XReports.Models
{
    public abstract partial class ReportSchema<TSourceEntity>
    {
        protected ReportCell[][] CreateComplexHeader(bool transpose = false)
        {
            SpannableHeader[,] header = this.BuildHeaderCells();
            this.MoveSpanHeaderTitleUp(header);

            return transpose ? this.GetTransposedHeaderCells(header) : this.GetHeaderCells(header);
        }

        private ReportCell[][] GetHeaderCells(SpannableHeader[,] header)
        {
            int rowsCount = header.GetLength(0);
            int columnsCount = header.GetLength(1);
            ReportCell[] row = new ReportCell[columnsCount];
            List<ReportCell[]> result = new List<ReportCell[]>();

            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    row[columnIndex] = header[rowIndex, columnIndex].Cell;
                }

                result.Add(row.ToArray());
            }

            return result.ToArray();
        }

        private ReportCell[][] GetTransposedHeaderCells(SpannableHeader[,] header)
        {
            int rowsCount = header.GetLength(0);
            int columnsCount = header.GetLength(1);
            ReportCell[] row = new ReportCell[rowsCount];
            List<ReportCell[]> result = new List<ReportCell[]>();

            for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                {
                    row[rowIndex] = header[rowIndex, columnIndex].Cell;

                    if (row[rowIndex] != null)
                    {
                        int span = row[rowIndex].ColumnSpan;
                        row[rowIndex].ColumnSpan = row[rowIndex].RowSpan;
                        row[rowIndex].RowSpan = span;
                    }
                }

                result.Add(row.ToArray());
            }

            return result.ToArray();
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
            ReportCell cell = ReportCell.FromValue(title);
            cell.ColumnSpan = columnSpan;

            foreach (ReportCellProperty property in this.CommonComplexHeaderProperties)
            {
                cell.AddProperty(property);
            }

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

            header[rowIndex, columnIndex] = header[(int)filledRowIndex, columnIndex];
            header[rowIndex, columnIndex].Cell.RowSpan = (int)filledRowIndex - rowIndex + 1;

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

        private class SpannableHeader
        {
            public static readonly SpannableHeader Spanned = new SpannableHeader(null) { IsSpanned = true };

            public SpannableHeader(ReportCell cell)
            {
                this.Cell = cell;
            }

            public ReportCell Cell { get; set; }

            public bool IsSpanned { get; private set; }
        }
    }
}
