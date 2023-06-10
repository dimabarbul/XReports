using XReports.Table;

namespace XReports.DocsSamples.Common;

public class SimpleConsoleWriter
{
    private const int ColumnWidth = 20;
    private const int SeparatorWidth = 1;
    private const int CellPaddingWidth = 2;
    private const string Separator = "|";

    public virtual void Write(IReportTable<ReportCell> reportTable)
    {
        int columnCount = 0;
        // The only way to get count of columns is to enumerate row.
        // But enumerating row recomputes all cells, so it's better to count
        // cells during processing any row.
        foreach (IEnumerable<ReportCell> headerRow in reportTable.HeaderRows)
        {
            columnCount = this.WriteRow(headerRow);
        }

        // Horizontal report can have no header.
        if (columnCount > 0)
        {
            Console.WriteLine(new string('-', columnCount * (ColumnWidth + SeparatorWidth + CellPaddingWidth) + SeparatorWidth));
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
            columnIndex++;

            Console.Write(Separator);
            Console.Write(' ');
            this.WriteCell(reportCell);
            Console.Write(' ');
        }

        Console.WriteLine(Separator);

        return columnIndex;
    }

    private void WriteCell(ReportCell reportCell)
    {
        this.WriteCell(reportCell, ColumnWidth);
    }
}
