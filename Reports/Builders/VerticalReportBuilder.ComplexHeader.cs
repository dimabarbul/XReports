using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;
using Reports.Models.Cells;

namespace Reports.Builders
{
    public partial class VerticalReportBuilder<TSourceEntity>
    {
        private readonly List<ComplexHeader> complexHeader = new List<ComplexHeader>();

        public void AddComplexHeader(int rowIndex, string title, string fromColumn, string toColumn = null)
        {
            this.complexHeader.Add(new ComplexHeader()
            {
                RowIndex = rowIndex,
                Title = title,
                StartIndex = this.columns.FindIndex(c => c.Title == fromColumn),
                EndIndex = this.columns.FindIndex(c => c.Title == (toColumn ?? fromColumn)),
            });
        }

        public void AddComplexHeader(int rowIndex, string title, int fromColumn, int? toColumn = null)
        {
            this.complexHeader.Add(new ComplexHeader()
            {
                RowIndex = rowIndex,
                Title = title,
                StartIndex = fromColumn,
                EndIndex = toColumn ?? fromColumn,
            });
        }

        private void BuildHeader(ReportTable table)
        {
            SpannableHeader[,] header = this.BuildHeaderCells();

            this.MoveSpanHeaderTitleUp(header);

            table.HeaderRows = this.GetHeaderCells(header);
        }

        private SpannableHeader[,] BuildHeaderCells()
        {
            int complexHeaderRowsCount =
                this.complexHeader.Any() ?
                this.complexHeader.Max(h => h.RowIndex) + 1 :
                0;
            SpannableHeader[,] headerCells = new SpannableHeader[complexHeaderRowsCount + 1, this.columns.Count];

            this.AddComplexHeaderCells(headerCells);
            this.AddRegularHeaderCells(headerCells);

            return headerCells;
        }

        private void AddComplexHeaderCells(SpannableHeader[,] headerCells)
        {
            foreach (ComplexHeader header in this.complexHeader
                .OrderBy(h => h.RowIndex)
                .ThenBy(h => h.StartIndex))
            {
                headerCells[header.RowIndex, header.StartIndex].Title = header.Title;

                for (int i = header.StartIndex + 1; i <= header.EndIndex; i++)
                {
                    headerCells[header.RowIndex, i].MarkSpanned();
                }
            }
        }

        private void AddRegularHeaderCells(SpannableHeader[,] headerCells)
        {
            int lastHeaderRowIndex = headerCells.GetLength(0) - 1;
            for (int i = 0; i < this.columns.Count; i++)
            {
                headerCells[lastHeaderRowIndex, i].Title = this.columns[i].Title;
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
                    if (header[rowIndex, columnIndex].Title != null || header[rowIndex, columnIndex].IsSpanned)
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

            for (int i = rowIndex + 1; i <= filledRowIndex; i++)
            {
                header[i, columnIndex].MarkSpanned();
            }
        }

        private int? GetNextFilledRowIndex(SpannableHeader[,] header, int rowIndex, int columnIndex)
        {
            int rowsCount = header.GetLength(0);
            for (int currentRowIndex = rowIndex + 1; currentRowIndex < rowsCount; currentRowIndex++)
            {
                if (header[currentRowIndex, columnIndex].Title != null && !header[currentRowIndex, columnIndex].IsSpanned)
                {
                    return currentRowIndex;
                }
            }

            return null;
        }

        private IEnumerable<IEnumerable<IReportCell>> GetHeaderCells(SpannableHeader[,] header)
        {
            int rowsCount = header.GetLength(0);
            int columnsCount = header.GetLength(1);
            IReportCell[] row = new IReportCell[columnsCount];

            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    row[columnIndex] = null;
                    if (header[rowIndex, columnIndex].IsSpanned || header[rowIndex, columnIndex].IsUsed)
                    {
                        continue;
                    }

                    row[columnIndex] = this.CreateHeaderCell(header, rowIndex, columnIndex);
                }

                yield return row;
            }
        }

        private ReportCell<string> CreateHeaderCell(SpannableHeader[,] header, int rowIndex, int columnIndex)
        {
            int rowSpan = this.CalculateRowSpan(header, rowIndex, columnIndex);
            int columnSpan = this.CalculateColumnSpan(header, rowIndex, columnIndex);
            if (rowSpan > 1)
            {
                columnSpan = 1;
            }

            for (int i = 1; i < rowSpan; i++)
            {
                header[rowIndex + i, columnIndex].MarkUsed();
            }

            for (int i = 1; i < columnSpan; i++)
            {
                header[rowIndex, columnIndex + i].MarkUsed();
            }

            return new ReportCell<string>(header[rowIndex, columnIndex].Title)
            {
                RowSpan = rowSpan,
                ColumnSpan = columnSpan
            };
        }

        private int CalculateRowSpan(SpannableHeader[,] header, int rowIndex, int columnIndex)
        {
            int rowSpan = 1;
            for (int i = rowIndex + 1; i < header.GetLength(0); i++)
            {
                if (!header[i, columnIndex].IsSpanned || header[i, columnIndex].IsUsed)
                {
                    break;
                }

                rowSpan++;
            }

            return rowSpan;
        }

        private int CalculateColumnSpan(SpannableHeader[,] header, int rowIndex, int columnIndex)
        {
            int columnSpan = 1;
            for (int i = columnIndex + 1; i < header.GetLength(1); i++)
            {
                if (!header[rowIndex, i].IsSpanned || header[rowIndex, i].IsUsed)
                {
                    break;
                }

                columnSpan++;
            }

            return columnSpan;
        }

        private struct SpannableHeader
        {
            public string Title { get; set; }
            public bool IsSpanned { get; private set; }
            public bool IsUsed { get; private set; }

            public void MarkSpanned()
            {
                this.Title = null;
                this.IsSpanned = true;
            }

            public void MarkUsed()
            {
                this.IsUsed = true;
            }
        }
    }

    internal class ComplexHeader
    {
        public int RowIndex { get; set; }
        public string Title { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}
