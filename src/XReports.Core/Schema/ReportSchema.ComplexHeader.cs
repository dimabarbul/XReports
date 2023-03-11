using XReports.Table;

namespace XReports.Schema
{
    internal abstract partial class ReportSchema<TSourceEntity>
    {
        protected ReportCell[][] CreateComplexHeader(bool isTransposed)
        {
            ComplexHeaderCell[,] headerCells = this.complexHeader;

            int height = headerCells.GetLength(0);
            int width = headerCells.GetLength(1);

            ReportCell[][] result = new ReportCell[height][];

            for (int i = 0; i < height; i++)
            {
                result[i] = new ReportCell[width];

                for (int j = 0; j < width; j++)
                {
                    if (headerCells[i, j] != null)
                    {
                        result[i][j] = headerCells[i, j].IsComplexHeaderCell ?
                            this.CreateComplexHeaderCell(headerCells[i, j]) :
                            this.CreateRegularHeaderCell(
                                isTransposed ? i : j,
                                headerCells[i, j]);
                    }
                }
            }

            return result;
        }

        private ReportCell CreateRegularHeaderCell(int cellsProviderIndex, ComplexHeaderCell headerCell)
        {
            ReportCell reportCell = this.Columns[cellsProviderIndex].CreateHeaderCell();
            reportCell.ColumnSpan = headerCell.ColumnSpan;
            reportCell.RowSpan = headerCell.RowSpan;

            return reportCell;
        }

        private ReportCell CreateComplexHeaderCell(ComplexHeaderCell headerCell)
        {
            ReportCell cell = ReportCell.FromValue(headerCell.Title);
            cell.ColumnSpan = headerCell.ColumnSpan;
            cell.RowSpan = headerCell.RowSpan;

            foreach (ReportCellProperty property in this.commonComplexHeaderProperties)
            {
                cell.AddProperty(property);
            }

            if (this.complexHeaderProperties.ContainsKey(headerCell.Title))
            {
                foreach (ReportCellProperty property in this.complexHeaderProperties[headerCell.Title])
                {
                    cell.AddProperty(property);
                }
            }

            return cell;
        }
    }
}
