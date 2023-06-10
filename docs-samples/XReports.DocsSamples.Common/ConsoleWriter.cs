using XReports.Table;

namespace XReports.DocsSamples.Common;

public class ConsoleWriter
{
    private const int ColumnWidth = 20;
    private const int SeparatorWidth = 1;
    private const int CellPaddingWidth = 2;
    private const string Separator = "|";
    private const char HorizontalSeparator = '-';

    private readonly SpannedCells spannedCells = new();

    public virtual void Write(IReportTable<ReportCell> reportTable)
    {
        // The only way to get count of columns is to enumerate row.
        // But enumerating row recomputes all cells, so it's better to count
        // cells during processing any row.
        foreach (IEnumerable<ReportCell> headerRow in reportTable.HeaderRows)
        {
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
            // spanned cell is null
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
        if (this.spannedCells.ShouldWriteEmptyCell(columnIndex))
        {
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

    private class SpannedCells
    {
        private readonly Dictionary<int, SpannedGroup> columns = new();
        private readonly Dictionary<Guid, SpannedGroup> allGroups = new();
        private Guid? currentGroupId;

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
                    foreach (int columnIndex in this.columns.Keys)
                    {
                        if (this.columns[columnIndex].Id == id)
                        {
                            this.columns.Remove(columnIndex);
                        }
                    }
                }
            }

            this.currentGroupId = null;
        }

        public bool IsSpanned(int columnIndex)
        {
            return this.columns.ContainsKey(columnIndex);
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
