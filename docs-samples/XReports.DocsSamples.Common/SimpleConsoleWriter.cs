using XReports.Table;

namespace XReports.DocsSamples.Common;

public class SimpleConsoleWriter
{
    private const int ColumnWidth = 20;
    private const int SeparatorWidth = 1;
    private const int CellPaddingWidth = 2;
    private const string Separator = "|";

    public void Write(IReportTable<ReportCell> reportTable)
    {
        int columnCount = 0;
        // The only way to get count of columns is to enumerate row.
        // But enumerating row recomputes all cells, so it's better to count
        // cells during processing any row.
        foreach (IEnumerable<ReportCell> headerRow in reportTable.HeaderRows)
        {
            columnCount = this.WriteRow(headerRow);
        }

        if (columnCount > 0)
        {
            Console.WriteLine(new string('-', columnCount * (ColumnWidth + SeparatorWidth + CellPaddingWidth) + SeparatorWidth));
        }

        foreach (IEnumerable<ReportCell> row in reportTable.Rows)
        {
            this.WriteRow(row);
        }
    }

    private int WriteRow(IEnumerable<ReportCell> row)
    {
        int columnIndex = 0;

        foreach (ReportCell reportCell in row)
        {
            columnIndex++;

            // spanned cell is null
            if (reportCell == null)
            {
                continue;
            }

            this.WriteSeparator();
            this.WritePadding();
            this.WriteCell(reportCell);
            this.WritePadding();
        }

        this.WriteSeparator();
        Console.WriteLine();

        return columnIndex;
    }

    private void WriteCell(ReportCell reportCell)
    {
        Console.Write($"{{0,{ColumnWidth}}}", reportCell.GetValue<string>());
    }

    private void WriteSeparator()
    {
        Console.Write(Separator);
    }

    private void WritePadding()
    {
        Console.Write(' ');
    }
}
