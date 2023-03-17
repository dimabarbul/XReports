using System.Collections.Generic;
using XReports.Schema;
using XReports.Table;

namespace XReports.Helpers
{
    public static class ComplexHeaderHelper
    {
        public static ReportCell[][] CreateCells<TSourceEntity>(
            IReadOnlyList<IReportColumn<TSourceEntity>> columns,
            ComplexHeaderCell[,] complexHeader,
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
