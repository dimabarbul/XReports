namespace XReports.Models
{
    public abstract partial class ReportSchema<TSourceEntity>
    {
        protected ReportCell[][] CreateComplexHeader(bool isTransposed)
        {
            ComplexHeaderCell[,] headerCells = this.ComplexHeader;

            ReportCell[][] result = new ReportCell[headerCells.GetLength(0)][];

            for (int i = 0; i < headerCells.GetLength(0); i++)
            {
                result[i] = new ReportCell[headerCells.GetLength(1)];

                for (int j = 0; j < headerCells.GetLength(1); j++)
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
            ReportCell reportCell = this.CellsProviders[cellsProviderIndex].CreateHeaderCell();
            reportCell.ColumnSpan = headerCell.ColumnSpan;
            reportCell.RowSpan = headerCell.RowSpan;

            return reportCell;
        }

        private ReportCell CreateComplexHeaderCell(ComplexHeaderCell headerCell)
        {
            ReportCell cell = ReportCell.FromValue(headerCell.Title);
            cell.ColumnSpan = headerCell.ColumnSpan;
            cell.RowSpan = headerCell.RowSpan;

            foreach (ReportCellProperty property in this.CommonComplexHeaderProperties)
            {
                cell.AddProperty(property);
            }

            if (this.ComplexHeaderProperties.ContainsKey(headerCell.Title))
            {
                foreach (ReportCellProperty property in this.ComplexHeaderProperties[headerCell.Title])
                {
                    cell.AddProperty(property);
                }
            }

            return cell;
        }
    }
}
