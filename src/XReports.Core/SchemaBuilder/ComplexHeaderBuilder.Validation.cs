using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Schema;

namespace XReports.SchemaBuilder
{
    public partial class ComplexHeaderBuilder : IComplexHeaderBuilder
    {
        private void Validate(IReadOnlyList<string> columnNames, IReadOnlyList<ColumnId> columnIds)
        {
            GroupWithPosition[] groupsWithPositions = this.groups
                .Select(g => this.GetGroupWithPosition(g, columnNames, columnIds))
                .ToArray();

            this.ValidateColumnIndexes(groupsWithPositions, columnNames.Count);
        }

        private void ValidateColumnIndexes(GroupWithPosition[] groupsWithPositions, int columnsCount)
        {
            foreach (GroupWithPosition group in groupsWithPositions)
            {
                if (group.EndIndex >= columnsCount)
                {
                    throw new ArgumentException(
                        $"Group {group.Group.Title} has too big end index {group.EndIndex}, columns count: {columnsCount}");
                }
            }
        }

        private void ValidateAllCellsFilled(ComplexHeaderCell[,] header)
        {
            for (int i = 0; i < header.GetLength(0); i++)
            {
                for (int j = 0; j < header.GetLength(1); j++)
                {
                    if (header[i, j] == null)
                    {
                        throw new ArgumentException($"Cell ({i + 1};{j + 1}) is unrelated");
                    }
                }
            }
        }

        private void ValidateNull(ComplexHeaderCell headerCell, string errorMessage)
        {
            if (headerCell != null)
            {
                throw new ArgumentException(errorMessage);
            }
        }
    }
}
