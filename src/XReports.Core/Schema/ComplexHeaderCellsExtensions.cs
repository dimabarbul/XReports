using System.Collections.Generic;
using XReports.Table;

namespace XReports.Schema
{
    /// <summary>
    /// Extensions for complex header which is a two-dimensional array of complex header cells.
    /// </summary>
    public static class ComplexHeaderCellsExtensions
    {
        /// <summary>
        /// Transposes complex header, so columns become rows and vice versa.
        /// </summary>
        /// <param name="header">Complex header to transpose.</param>
        /// <returns>Transposed complex header.</returns>
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
                        Content = header[rowIndex, columnIndex].Content,
                        ColumnSpan = header[rowIndex, columnIndex].RowSpan,
                        RowSpan = header[rowIndex, columnIndex].ColumnSpan,
                        IsComplexHeaderCell = header[rowIndex, columnIndex].IsComplexHeaderCell,
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// Creates report cells for complex header.
        /// </summary>
        /// <param name="complexHeader">Complex header to create cells for.</param>
        /// <param name="columns">Report columns.</param>
        /// <param name="complexHeaderProperties">Complex header properties. Dictionary has following structure: key is content of cell to apply properties, value - properties to apply. Applies only to cells generated from complex header groups, not from report column headers.</param>
        /// <param name="commonComplexHeaderProperties">Properties to apply to all complex header cells. Applies only to cells generated from complex header groups, not from report column headers.</param>
        /// <param name="isTransposed">Is complex header transposed. This impacts how report column is determined for complex header cell.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <returns>Report cells for complex header.</returns>
        public static ReportCell[][] CreateCells<TSourceItem>(
            this ComplexHeaderCell[,] complexHeader,
            IReadOnlyList<IReportColumn<TSourceItem>> columns,
            IReadOnlyDictionary<string, IReportCellProperty[]> complexHeaderProperties,
            IReadOnlyList<IReportCellProperty> commonComplexHeaderProperties,
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
            IReadOnlyList<IReportCellProperty> commonComplexHeaderProperties,
            IReadOnlyDictionary<string, IReportCellProperty[]> complexHeaderProperties,
            ComplexHeaderCell headerCell)
        {
            ReportCell cell = new ReportCell();
            cell.SetValue(headerCell.Content);
            cell.ColumnSpan = headerCell.ColumnSpan;
            cell.RowSpan = headerCell.RowSpan;

            foreach (IReportCellProperty property in commonComplexHeaderProperties)
            {
                cell.AddProperty(property);
            }

            if (complexHeaderProperties.ContainsKey(headerCell.Content))
            {
                foreach (IReportCellProperty property in complexHeaderProperties[headerCell.Content])
                {
                    cell.AddProperty(property);
                }
            }

            return cell;
        }
    }
}
