using XReports.Table;

namespace XReports.DocsSamples.Common;

/// <summary>
/// Writer of generic report to console.
/// </summary>
public class ConsoleWriter
{
    private const int ColumnWidth = 20;
    private const int SeparatorWidth = 1;
    private const int CellPaddingWidth = 2;
    private const string Separator = "|";
    private const char HorizontalSeparator = '-';

    /// <summary>
    /// Allows tracking spanned cells. This impacts writing vertical and
    /// horizontal separators.
    /// </summary>
    /// <remarks>
    /// There are some cases when separator should not be written:
    /// <list type="bullet">
    /// <item><description>
    /// if cell has ColumnSpan greater than 1, it and next (ColumnSpan - 1)
    /// cells in the same row should not have right separator
    /// </description></item>
    /// <item><description>
    /// if cell has RowSpan greater than 1, it and next (RowSpan - 1) cells in
    /// the same column should not have bottom separator
    /// </description></item>
    /// </list>
    /// </remarks>
    private readonly SpannedCells spannedCells = new();

    public virtual void Write(IReportTable<ReportCell> reportTable)
    {
        foreach (IEnumerable<ReportCell> headerRow in reportTable.HeaderRows)
        {
            // The only way to get count of columns is to enumerate row.
            // But enumerating row recomputes all cells, so it's better to count
            // cells during processing any row.
            int columnCount = this.WriteRow(headerRow);
            this.WriteHeaderSeparator(columnCount);
        }

        foreach (IEnumerable<ReportCell> row in reportTable.Rows)
        {
            this.WriteRow(row);
        }
    }

    protected virtual void WriteCell(ReportCell reportCell, int cellWidth)
    {
        Console.Write($"{{0,{cellWidth}}}", reportCell.GetValue<string>());
    }

    private int WriteRow(IEnumerable<ReportCell> row)
    {
        int columnIndex = 0;

        foreach (ReportCell reportCell in row)
        {
            // When cell has ColumnSpan greater than 1, next (ColumnSpan - 1)
            // cells in the same row will be null.
            // Likewise, when cell ahs RowSpan greater tha 1, next (RowSpan - 1)
            // cells in the same column will be null.
            if (reportCell == null)
            {
                this.WriteSpannedCell(columnIndex);

                columnIndex++;
                continue;
            }

            if (reportCell.RowSpan > 1 || reportCell.ColumnSpan > 1)
            {
                this.spannedCells.AddGroup(columnIndex, reportCell.ColumnSpan, reportCell.RowSpan);
            }

            this.WriteSeparator();
            this.WritePadding();
            this.WriteCell(reportCell);
            this.WritePadding();

            columnIndex++;
        }

        this.WriteSeparator();

        Console.WriteLine();

        this.spannedCells.MoveToNextRow();

        return columnIndex;
    }

    private void WriteSpannedCell(int columnIndex)
    {
        // If the cell we're about to write is a part of previously spanned
        // group, we should not write anything as the spanning cell occupies
        // width of all spanned cells.
        if (this.spannedCells.ShouldWriteEmptyCell(columnIndex))
        {
            // Separator should be written if the cell is the first in its
            // spanned group.
            if (this.spannedCells.ShouldWriteSeparatorBeforeCell(columnIndex))
            {
                this.WriteSeparator();
            }
            else
            {
                this.WriteEmptySeparator();
            }

            this.WriteEmptyCell();
        }
    }

    private void WriteCell(ReportCell reportCell)
    {
        int cellWidth = ColumnWidth + (reportCell.ColumnSpan - 1) * (ColumnWidth + SeparatorWidth + CellPaddingWidth);
        this.WriteCell(reportCell, cellWidth);
    }

    private void WriteHeaderSeparator(int columnCount)
    {
        for (int i = 0; i < columnCount; i++)
        {
            if (this.spannedCells.IsSpanned(i))
            {
                if (this.spannedCells.ShouldWriteSeparatorBeforeCell(i))
                {
                    this.WriteSeparator();
                }
                else
                {
                    this.WriteEmptySeparator();
                }

                this.WriteEmptyCell();
            }
            else
            {
                this.WriteSeparator();
                this.WriteCellHorizontalSeparator();
            }
        }

        this.WriteSeparator();
        Console.WriteLine();
    }

    private void WriteSeparator()
    {
        Console.Write(Separator);
    }

    private void WriteEmptySeparator()
    {
        Console.Write($"{{0,{SeparatorWidth}}}", string.Empty);
    }

    private void WritePadding()
    {
        Console.Write(' ');
    }

    private void WriteEmptyCell()
    {
        Console.Write($"{{0,{ColumnWidth + CellPaddingWidth}}}", string.Empty);
    }

    private void WriteCellHorizontalSeparator()
    {
        Console.Write(new string(HorizontalSeparator, ColumnWidth + CellPaddingWidth));
    }

    /// <summary>
    /// Tracks currently spanned cells.
    /// </summary>
    private class SpannedCells
    {
        private readonly Dictionary<int, SpannedGroup> columns = new();
        private readonly Dictionary<Guid, SpannedGroup> allGroups = new();
        private Guid? currentGroupId;

        public void AddGroup(int columnIndex, int columnSpan, int rowSpan)
        {
            Guid id = Guid.NewGuid();
            SpannedGroup group = new(id, rowSpan);
            for (int i = 0; i < columnSpan; i++)
            {
                this.columns[columnIndex + i] = group;
            }

            this.allGroups[id] = group;

            this.currentGroupId = id;
        }

        public void MoveToNextRow()
        {
            foreach (Guid id in this.allGroups.Keys)
            {
                if (!this.allGroups[id].MoveToNextRow())
                {
                    this.RemoveGroup(id);
                }
            }

            this.currentGroupId = null;
        }

        public bool ShouldWriteEmptyCell(int columnIndex)
        {
            return !this.columns.ContainsKey(columnIndex)
                || this.columns[columnIndex].Id != this.currentGroupId;
        }

        public bool ShouldWriteSeparatorBeforeCell(int columnIndex)
        {
            if (!this.columns.ContainsKey(columnIndex - 1))
            {
                return true;
            }

            if (!this.columns.ContainsKey(columnIndex))
            {
                return true;
            }

            return this.columns[columnIndex - 1].Id != this.columns[columnIndex].Id;
        }

        public bool IsSpanned(int columnIndex)
        {
            return this.columns.ContainsKey(columnIndex);
        }

        private void RemoveGroup(Guid id)
        {
            foreach (int columnIndex in this.columns.Keys)
            {
                if (this.columns[columnIndex].Id == id)
                {
                    this.columns.Remove(columnIndex);
                }
            }

            this.allGroups.Remove(id);
        }

        private class SpannedGroup
        {
            private int rowSpan;

            public Guid Id { get; }

            public SpannedGroup(Guid id, int rowSpan)
            {
                this.rowSpan = rowSpan;
                this.Id = id;
            }

            public bool MoveToNextRow()
            {
                this.rowSpan--;

                return this.rowSpan > 0;
            }
        }
    }
}
