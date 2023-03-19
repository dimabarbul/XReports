using System.Collections.Generic;
using XReports.Table;

namespace XReports.Schema
{
    public static class ComplexHeaderCellsExtensions
    {
        public static ComplexHeaderCell[,] Transpose(this ComplexHeaderCell[,] header)
        {
            int rowsCount = header.GetLength(0);
            int columnsCount = header.GetLength(1);
            ComplexHeaderCell[,] result = new ComplexHeaderCell[columnsCount, rowsCount];

            for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                {
                    if (header[rowIndex, columnIndex] == null)
                    {
                        continue;
                    }

                    result[columnIndex, rowIndex] = new ComplexHeaderCell()
                    {
                        Title = header[rowIndex, columnIndex].Title,
                        ColumnSpan = header[rowIndex, columnIndex].RowSpan,
                        RowSpan = header[rowIndex, columnIndex].ColumnSpan,
                        IsComplexHeaderCell = header[rowIndex, columnIndex].IsComplexHeaderCell,
                    };
                }
            }

            return result;
        }

        public static ReportCell[][] CreateCells<TSourceEntity>(
            this ComplexHeaderCell[,] complexHeader,
            IReadOnlyList<IReportColumn<TSourceEntity>> columns,
            IReadOnlyDictionary<string, ReportCellProperty[]> complexHeaderProperties,
            IReadOnlyList<ReportCellProperty> commonComplexHeaderProperties,
            bool isTransposed)
        {
            int height = complexHeader.GetLength(0);
            int width = complexHeader.GetLength(1);

            ReportCell[][] result = new ReportCell[height][];

            for (int i = 0; i < height; i++)
            {
                result[i] = new ReportCell[width];

                for (int j = 0; j < width; j++)
                {
                    if (complexHeader[i, j] != null)
                    {
                        result[i][j] = complexHeader[i, j].IsComplexHeaderCell ?
                            CreateComplexHeaderCell(
                                commonComplexHeaderProperties,
                                complexHeaderProperties,
                                complexHeader[i, j]) :
                            CreateRegularHeaderCell(
                                columns[isTransposed ? i : j],
                                complexHeader[i, j]);
                    }
                }
            }

            return result;
        }

        private static ReportCell CreateRegularHeaderCell<TSourceEntity>(IReportColumn<TSourceEntity> column, ComplexHeaderCell headerCell)
        {
            ReportCell reportCell = column.CreateHeaderCell();
            reportCell.ColumnSpan = headerCell.ColumnSpan;
            reportCell.RowSpan = headerCell.RowSpan;

            return reportCell;
        }

        private static ReportCell CreateComplexHeaderCell(
            IReadOnlyList<ReportCellProperty> commonComplexHeaderProperties,
            IReadOnlyDictionary<string, ReportCellProperty[]> complexHeaderProperties,
            ComplexHeaderCell headerCell)
        {
            ReportCell cell = ReportCell.FromValue(headerCell.Title);
            cell.ColumnSpan = headerCell.ColumnSpan;
            cell.RowSpan = headerCell.RowSpan;

            foreach (ReportCellProperty property in commonComplexHeaderProperties)
            {
                cell.AddProperty(property);
            }

            if (complexHeaderProperties.ContainsKey(headerCell.Title))
            {
                foreach (ReportCellProperty property in complexHeaderProperties[headerCell.Title])
                {
                    cell.AddProperty(property);
                }
            }

            return cell;
        }
    }
}
