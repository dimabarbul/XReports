using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Models;

namespace XReports
{
    public partial class ComplexHeaderBuilder
    {
        private readonly ComplexHeaderCell spannedCell = new ComplexHeaderCell();
        private readonly List<ComplexHeaderGroup> groups = new List<ComplexHeaderGroup>();

        public void AddGroup(int rowIndex, string title, int fromColumn, int? toColumn = null)
        {
            this.ValidateNumberNotNegative(nameof(rowIndex), rowIndex);
            this.ValidateNumberNotNegative(nameof(fromColumn), fromColumn);

            if (toColumn != null)
            {
                this.ValidateNumberNotNegative(nameof(toColumn), (int)toColumn);
            }

            this.groups.Add(
                new ComplexHeaderGroupByIndex(
                    rowIndex,
                    title,
                    fromColumn,
                    toColumn ?? fromColumn,
                    1));
        }

        public void AddGroup(int rowIndex, string title, string fromColumn, string toColumn = null)
        {
            this.ValidateNumberNotNegative(nameof(rowIndex), rowIndex);

            this.groups.Add(
                new ComplexHeaderGroupByName(
                    rowIndex,
                    title,
                    fromColumn,
                    toColumn,
                    1
                ));
        }

        public void AddGroup(int rowIndex, int rowSpan, string title, int fromColumn, int? toColumn = null)
        {
            this.ValidateNumberNotNegative(nameof(rowIndex), rowIndex);
            this.ValidateNumberNotNegative(nameof(fromColumn), fromColumn);
            this.ValidateNumberPositive(nameof(rowSpan), rowSpan);

            if (toColumn != null)
            {
                this.ValidateNumberNotNegative(nameof(toColumn), (int)toColumn);
            }

            this.groups.Add(
                new ComplexHeaderGroupByIndex(
                    rowIndex,
                    title,
                    fromColumn,
                    toColumn ?? fromColumn,
                    rowSpan));
        }

        public void AddGroup(int rowIndex, int rowSpan, string title, string fromColumn, string toColumn = null)
        {
            this.ValidateNumberNotNegative(nameof(rowIndex), rowIndex);
            this.ValidateNumberPositive(nameof(rowSpan), rowSpan);

            this.groups.Add(
                new ComplexHeaderGroupByName(
                    rowIndex,
                    title,
                    fromColumn,
                    toColumn,
                    rowSpan
                ));
        }

        public ComplexHeaderCell[,] Build(IReadOnlyList<string> columnNames)
        {
            this.Validate(columnNames);
            this.NormalizeRowIndexes();
            ComplexHeaderCell[,] header = this.BuildHeaderCells(columnNames);
            this.MoveSpanHeaderTitleUp(header);
            this.ValidateAllCellsFilled(header);
            this.CleanUpHeaderCells(header);

            return header;
        }

        /// <summary>
        /// Makes minimum index 0 and all indexes to go without gaps.
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

        private ComplexHeaderCell[,] BuildHeaderCells(IReadOnlyList<string> columnNames)
        {
            int complexHeaderRowsCount =
                this.groups.Count > 0 ?
                    this.groups.Max(h => h.RowIndex + h.RowSpan - 1) + 1 :
                    0;
            ComplexHeaderCell[,] headerCells = new ComplexHeaderCell[complexHeaderRowsCount + 1, columnNames.Count];

            this.AddComplexHeaderCells(headerCells, columnNames);
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
                    if (ReferenceEquals(header[rowIndex, columnIndex], this.spannedCell))
                    {
                        header[rowIndex, columnIndex] = null;
                    }
                }
            }
        }

        private void AddComplexHeaderCells(ComplexHeaderCell[,] headerCells, IReadOnlyList<string> columnNames)
        {
            foreach (GroupWithPosition group in this.groups
                .Select(g => this.GetGroupWithPosition(g, columnNames))
                .OrderBy(h => h.Group.RowIndex)
                .ThenBy(h => h.StartIndex))
            {
                string errorMessage = $"Group {group.Group.Title} overlaps with another group";

                this.ValidateNull(headerCells[group.Group.RowIndex, group.StartIndex], errorMessage);
                headerCells[group.Group.RowIndex, group.StartIndex] = new ComplexHeaderCell()
                {
                    Title = group.Group.Title,
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

        private GroupWithPosition GetGroupWithPosition(ComplexHeaderGroup group, IReadOnlyList<string> columnNames)
        {
            if (group is ComplexHeaderGroupByIndex groupByIndex)
            {
                return new GroupWithPosition(group, groupByIndex.StartIndex, groupByIndex.EndIndex);
            }

            ComplexHeaderGroupByName groupByName = (ComplexHeaderGroupByName)group;

            return new GroupWithPosition(group,
                this.GetColumnIndex(groupByName.FromColumn, columnNames),
                this.GetColumnIndex(groupByName.ToColumn, columnNames));
        }

        private int GetColumnIndex(string columnName, IReadOnlyList<string> columnNames)
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

        private void AddRegularHeaderCells(ComplexHeaderCell[,] headerCells, IReadOnlyList<string> columnNames)
        {
            int lastHeaderRowIndex = headerCells.GetLength(0) - 1;
            for (int i = 0; i < columnNames.Count; i++)
            {
                headerCells[lastHeaderRowIndex, i] = new ComplexHeaderCell()
                {
                    Title = columnNames[i],
                    IsComplexHeaderCell = false,
                    ColumnSpan = 1,
                    RowSpan = 1,
                };
            }
        }

        private void MoveSpanHeaderTitleUp(ComplexHeaderCell[,] header)
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

                    this.MoveTitleUpForEmptyCell(header, rowIndex, columnIndex);
                }
            }
        }

        private void MoveTitleUpForEmptyCell(ComplexHeaderCell[,] header, int rowIndex, int columnIndex)
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
            public int RowIndex { get; set; }
            public string Title { get; }
            public int RowSpan { get; }

            protected ComplexHeaderGroup(int rowIndex, string title, int rowSpan)
            {
                this.RowIndex = rowIndex;
                this.Title = title;
                this.RowSpan = rowSpan;
            }
        }

        private class ComplexHeaderGroupByName : ComplexHeaderGroup
        {
            public string FromColumn { get; }
            public string ToColumn { get; }

            public ComplexHeaderGroupByName(int rowIndex, string title, string fromColumn, string toColumn, int rowSpan)
                : base(rowIndex, title, rowSpan)
            {
                this.FromColumn = fromColumn;
                this.ToColumn = toColumn ?? fromColumn;
            }
        }

        private class ComplexHeaderGroupByIndex : ComplexHeaderGroup
        {
            public int StartIndex { get; }
            public int EndIndex { get; }

            public ComplexHeaderGroupByIndex(int rowIndex, string title, int startIndex, int endIndex, int rowSpan)
                : base(rowIndex, title, rowSpan)
            {
                this.StartIndex = startIndex;
                this.EndIndex = endIndex;
            }
        }

        private class GroupWithPosition
        {
            public ComplexHeaderGroup Group { get; }
            public int StartIndex { get; }
            public int EndIndex { get; }

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
        }
    }
}
