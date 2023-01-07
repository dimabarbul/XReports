using XReports.Models;

namespace XReports.Extensions
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
    }
}
