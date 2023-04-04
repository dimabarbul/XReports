using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Helpers;
using XReports.Schema;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Complex header builder.
    /// </summary>
    public partial class ComplexHeaderBuilder : IComplexHeaderBuilder
    {
        private readonly ComplexHeaderCell spannedCell = new ComplexHeaderCell();
        private readonly List<ComplexHeaderGroup> groups = new List<ComplexHeaderGroup>();

        /// <inheritdoc />
        public void AddGroup(int rowIndex, string content, int fromColumn, int? toColumn = null)
        {
            Validation.NotNegative(nameof(rowIndex), rowIndex);
            Validation.NotNegative(nameof(fromColumn), fromColumn);

            if (toColumn != null)
            {
                Validation.NotNegative(nameof(toColumn), (int)toColumn);
            }

            this.groups.Add(
                new ComplexHeaderGroupByIndex(
                    rowIndex,
                    content,
                    fromColumn,
                    toColumn ?? fromColumn,
                    1));
        }

        /// <inheritdoc />
        public void AddGroup(int rowIndex, string content, string fromColumn, string toColumn = null)
        {
            Validation.NotNegative(nameof(rowIndex), rowIndex);

            this.groups.Add(
                new ComplexHeaderGroupByName(
                    rowIndex,
                    content,
                    fromColumn,
                    toColumn,
                    1
                ));
        }

        /// <inheritdoc />
        public void AddGroup(int rowIndex, string content, ColumnId fromColumn, ColumnId toColumn = null)
        {
            Validation.NotNegative(nameof(rowIndex), rowIndex);

            this.groups.Add(
                new ComplexHeaderGroupByColumnId(
                    rowIndex,
                    content,
                    fromColumn,
                    toColumn,
                    1
                ));
        }

        /// <inheritdoc />
        public void AddGroup(int rowIndex, int rowSpan, string content, int fromColumn, int? toColumn = null)
        {
            Validation.NotNegative(nameof(rowIndex), rowIndex);
            Validation.NotNegative(nameof(fromColumn), fromColumn);
            Validation.Positive(nameof(rowSpan), rowSpan);

            if (toColumn != null)
            {
                Validation.NotNegative(nameof(toColumn), (int)toColumn);
            }

            this.groups.Add(
                new ComplexHeaderGroupByIndex(
                    rowIndex,
                    content,
                    fromColumn,
                    toColumn ?? fromColumn,
                    rowSpan));
        }

        /// <inheritdoc />
        public void AddGroup(int rowIndex, int rowSpan, string content, string fromColumn, string toColumn = null)
        {
            Validation.NotNegative(nameof(rowIndex), rowIndex);
            Validation.Positive(nameof(rowSpan), rowSpan);

            this.groups.Add(
                new ComplexHeaderGroupByName(
                    rowIndex,
                    content,
                    fromColumn,
                    toColumn,
                    rowSpan
                ));
        }

        /// <inheritdoc />
        public void AddGroup(int rowIndex, int rowSpan, string content, ColumnId fromColumn, ColumnId toColumn = null)
        {
            Validation.NotNegative(nameof(rowIndex), rowIndex);
            Validation.Positive(nameof(rowSpan), rowSpan);

            this.groups.Add(
                new ComplexHeaderGroupByColumnId(
                    rowIndex,
                    content,
                    fromColumn,
                    toColumn,
                    rowSpan
                ));
        }

        /// <inheritdoc />
        public ComplexHeaderCell[,] Build(IReadOnlyList<string> columnNames, IReadOnlyList<ColumnId> columnIds)
        {
            if (columnNames.Count != columnIds.Count)
            {
                throw new ArgumentException("Count of column names should be the same as count of column IDs.");
            }

            this.Validate(columnNames, columnIds);
            this.NormalizeRowIndexes();
            ComplexHeaderCell[,] header = this.BuildHeaderCells(columnNames, columnIds);
            this.MoveSpanHeaderContentUp(header);
            this.ValidateAllCellsFilled(header);
            this.CleanUpHeaderCells(header);

            return header;
        }

        private static GroupWithPosition GetGroupWithPosition(ComplexHeaderGroup group, IReadOnlyList<string> columnNames, IReadOnlyList<ColumnId> columnIds)
        {
            switch (group)
            {
                case ComplexHeaderGroupByIndex groupByIndex:
                    return new GroupWithPosition(group, groupByIndex.StartIndex, groupByIndex.EndIndex);
                case ComplexHeaderGroupByName groupByName:
                    return new GroupWithPosition(group, GetColumnIndex(groupByName.FromColumn, columnNames), GetColumnIndex(groupByName.ToColumn, columnNames));
                case ComplexHeaderGroupByColumnId groupByColumnId:
                    return new GroupWithPosition(group, GetColumnIndex(groupByColumnId.FromColumn, columnIds), GetColumnIndex(groupByColumnId.ToColumn, columnIds));
                default:
                    throw new ArgumentOutOfRangeException(nameof(group), $"Unexpected type of group: {group.GetType()}");
            }
        }

        private static int GetColumnIndex(string columnName, IReadOnlyList<string> columnNames)
        {
            for (int i = 0; i < columnNames.Count; i++)
            {
                if (columnNames[i].Equals(columnName, StringComparison.Ordinal))
                {
                    return i;
                }
            }

            throw new ArgumentException($"Column name {columnName} not found");
        }

        private static int GetColumnIndex(ColumnId columnId, IReadOnlyList<ColumnId> columnIds)
        {
            for (int i = 0; i < columnIds.Count; i++)
            {
                if (columnId.Equals(columnIds[i]))
                {
                    return i;
                }
            }

            throw new ArgumentException($"Column ID {columnId} not found");
        }

        /// <summary>
        /// Normalizes indexes in the groups, i.e. makes minimum index 0 and
        /// all indexes to go without gaps.
        /// For example, if groups have indexes: 1, 3, 4, 6, after this method they will be:
        /// 0, 1, 2, 3.
        /// </summary>
        private void NormalizeRowIndexes()
        {
            if (this.groups.Count == 0)
            {
                return;
            }

            Dictionary<int, int> rowIndexes = this.groups
                .Select(g => g.RowIndex)
                .Distinct()
                .OrderBy(i => i)
                .Select((rowIndex, newIndex) => (rowIndex, newIndex))
                .ToDictionary(x => x.rowIndex, x => x.newIndex);

            foreach (ComplexHeaderGroup group in this.groups)
            {
                group.RowIndex = rowIndexes[group.RowIndex];
            }
        }

        private ComplexHeaderCell[,] BuildHeaderCells(IReadOnlyList<string> columnNames, IReadOnlyList<ColumnId> columnIds)
        {
            int complexHeaderRowsCount =
                this.groups.Count > 0 ?
                    this.groups.Max(h => h.RowIndex + h.RowSpan - 1) + 1 :
                    0;
            ComplexHeaderCell[,] headerCells = new ComplexHeaderCell[complexHeaderRowsCount + 1, columnNames.Count];

            this.AddComplexHeaderCells(headerCells, columnNames, columnIds);
            this.AddRegularHeaderCells(headerCells, columnNames);

            return headerCells;
        }

        private void CleanUpHeaderCells(ComplexHeaderCell[,] header)
        {
            int rowsCount = header.GetLength(0);
            int columnsCount = header.GetLength(1);

            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    if (header[rowIndex, columnIndex] == this.spannedCell)
                    {
                        header[rowIndex, columnIndex] = null;
                    }
                }
            }
        }

        private void AddComplexHeaderCells(ComplexHeaderCell[,] headerCells, IReadOnlyList<string> columnNames, IReadOnlyList<ColumnId> columnIds)
        {
            foreach (GroupWithPosition group in this.groups
                .Select(g => GetGroupWithPosition(g, columnNames, columnIds))
                .OrderBy(h => h.Group.RowIndex)
                .ThenBy(h => h.StartIndex))
            {
                string errorMessage = $"Group {group.Group.Content} overlaps with another group";

                this.ValidateNull(headerCells[group.Group.RowIndex, group.StartIndex], errorMessage);
                headerCells[group.Group.RowIndex, group.StartIndex] = new ComplexHeaderCell()
                {
                    Content = group.Group.Content,
                    ColumnSpan = group.EndIndex - group.StartIndex + 1,
                    RowSpan = group.Group.RowSpan,
                    IsComplexHeaderCell = true,
                };

                for (int i = group.StartIndex + 1; i <= group.EndIndex; i++)
                {
                    for (int j = 0; j < group.Group.RowSpan; j++)
                    {
                        this.ValidateNull(headerCells[group.Group.RowIndex + j, i], errorMessage);
                        headerCells[group.Group.RowIndex + j, i] = this.spannedCell;
                    }
                }

                for (int j = 1; j < group.Group.RowSpan; j++)
                {
                    this.ValidateNull(headerCells[group.Group.RowIndex + j, group.StartIndex], errorMessage);

                    headerCells[group.Group.RowIndex + j, group.StartIndex] = this.spannedCell;
                }
            }
        }

        private void AddRegularHeaderCells(ComplexHeaderCell[,] headerCells, IReadOnlyList<string> columnNames)
        {
            int lastHeaderRowIndex = headerCells.GetLength(0) - 1;
            for (int i = 0; i < columnNames.Count; i++)
            {
                headerCells[lastHeaderRowIndex, i] = new ComplexHeaderCell()
                {
                    Content = columnNames[i],
                    IsComplexHeaderCell = false,
                    ColumnSpan = 1,
                    RowSpan = 1,
                };
            }
        }

        private void MoveSpanHeaderContentUp(ComplexHeaderCell[,] header)
        {
            int rowsCount = header.GetLength(0);
            int columnsCount = header.GetLength(1);
            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    if (header[rowIndex, columnIndex] != null)
                    {
                        continue;
                    }

                    this.MoveContentUpForEmptyCell(header, rowIndex, columnIndex);
                }
            }
        }

        private void MoveContentUpForEmptyCell(ComplexHeaderCell[,] header, int rowIndex, int columnIndex)
        {
            int? filledRowIndex = this.GetNextFilledRowIndex(header, rowIndex, columnIndex);
            if (filledRowIndex == null)
            {
                return;
            }

            if (header[(int)filledRowIndex, columnIndex] != null &&
                header[(int)filledRowIndex, columnIndex].IsComplexHeaderCell)
            {
                return;
            }

            header[rowIndex, columnIndex] = header[(int)filledRowIndex, columnIndex];
            header[rowIndex, columnIndex].RowSpan = (int)filledRowIndex - rowIndex + 1;

            for (int i = rowIndex + 1; i <= filledRowIndex; i++)
            {
                header[i, columnIndex] = this.spannedCell;
            }
        }

        private int? GetNextFilledRowIndex(ComplexHeaderCell[,] header, int rowIndex, int columnIndex)
        {
            int rowsCount = header.GetLength(0);
            for (int currentRowIndex = rowIndex + 1; currentRowIndex < rowsCount; currentRowIndex++)
            {
                if (header[currentRowIndex, columnIndex] != null &&
                    header[currentRowIndex, columnIndex] != this.spannedCell)
                {
                    return currentRowIndex;
                }
            }

            return null;
        }

        private abstract class ComplexHeaderGroup
        {
            protected ComplexHeaderGroup(int rowIndex, string content, int rowSpan)
            {
                this.RowIndex = rowIndex;
                this.Content = content;
                this.RowSpan = rowSpan;
            }

            public int RowIndex { get; set; }
            public string Content { get; }
            public int RowSpan { get; }
        }

        private class ComplexHeaderGroupByName : ComplexHeaderGroup
        {
            public ComplexHeaderGroupByName(int rowIndex, string content, string fromColumn, string toColumn, int rowSpan)
                : base(rowIndex, content, rowSpan)
            {
                Validation.NotNull(nameof(fromColumn), fromColumn);

                this.FromColumn = fromColumn;
                this.ToColumn = toColumn ?? fromColumn;
            }

            public string FromColumn { get; }
            public string ToColumn { get; }
        }

        private class ComplexHeaderGroupByIndex : ComplexHeaderGroup
        {
            public ComplexHeaderGroupByIndex(int rowIndex, string content, int startIndex, int endIndex, int rowSpan)
                : base(rowIndex, content, rowSpan)
            {
                this.StartIndex = startIndex;
                this.EndIndex = endIndex;
            }

            public int StartIndex { get; }
            public int EndIndex { get; }
        }

        private class ComplexHeaderGroupByColumnId : ComplexHeaderGroup
        {
            public ComplexHeaderGroupByColumnId(int rowIndex, string content, ColumnId fromColumn, ColumnId toColumn, int rowSpan)
                : base(rowIndex, content, rowSpan)
            {
                Validation.NotNull(nameof(fromColumn), fromColumn);

                this.FromColumn = fromColumn;
                this.ToColumn = toColumn ?? fromColumn;
            }

            public ColumnId FromColumn { get; }
            public ColumnId ToColumn { get; }
        }

        private class GroupWithPosition
        {
            public GroupWithPosition(ComplexHeaderGroup group, int startIndex, int endIndex)
            {
                this.Group = group;

                if (startIndex <= endIndex)
                {
                    (this.StartIndex, this.EndIndex) = (startIndex, endIndex);
                }
                else
                {
                    (this.StartIndex, this.EndIndex) = (endIndex, startIndex);
                }
            }

            public ComplexHeaderGroup Group { get; }
            public int StartIndex { get; }
            public int EndIndex { get; }
        }
    }
}
