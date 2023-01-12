using System;
using System.Linq;
using FluentAssertions;
using XReports.Models;
using Xunit;

namespace XReports.Core.Tests
{
    public class ComplexHeaderBuilderTest
    {
        private readonly ComplexHeaderBuilder builder = new ComplexHeaderBuilder();
        private readonly string[] columnNames = { "Column1", "Column2", "Column3", "Column4", "Column5" };

        [Fact]
        public void AddGroupByColumnIndexesShouldAddHeaderWithMultipleGroupsInOneRow()
        {
            this.builder.AddGroup(0, "Group1", 0, 1);
            this.builder.AddGroup(0, "Group2", 2, 3);

            ComplexHeaderCell[,] cells = this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 4));

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group1", columnSpan: 2),
                    null,
                    this.CreateComplexCell("Group2", columnSpan: 2),
                    null,
                },
                {
                    this.CreateRegularCell(this.columnNames[0]),
                    this.CreateRegularCell(this.columnNames[1]),
                    this.CreateRegularCell(this.columnNames[2]),
                    this.CreateRegularCell(this.columnNames[3]),
                },
            });
        }

        [Fact]
        public void AddGroupByColumnNamesShouldAddHeaderWithMultipleGroupsInOneRow()
        {
            this.builder.AddGroup(0, "Group1", "Column1", "Column2");
            this.builder.AddGroup(0, "Group2", "Column3", "Column4");

            ComplexHeaderCell[,] cells = this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 4));

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group1", columnSpan: 2),
                    null,
                    this.CreateComplexCell("Group2", columnSpan: 2),
                    null,
                },
                {
                    this.CreateRegularCell(this.columnNames[0]),
                    this.CreateRegularCell(this.columnNames[1]),
                    this.CreateRegularCell(this.columnNames[2]),
                    this.CreateRegularCell(this.columnNames[3]),
                },
            });
        }

        [Fact]
        public void AddGroupByColumnIndexesShouldAddHeaderNotGroupingAllColumns()
        {
            this.builder.AddGroup(0, "Group", 1, 2);

            ComplexHeaderCell[,] cells = this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 3));

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateRegularCell("Column1", rowSpan: 2),
                    this.CreateComplexCell("Group", columnSpan: 2),
                    null,
                },
                {
                    null,
                    this.CreateRegularCell("Column2"),
                    this.CreateRegularCell("Column3"),
                },
            });
        }

        [Fact]
        public void AddGroupByColumnNamesShouldAddHeaderNotGroupingAllColumns()
        {
            this.builder.AddGroup(0, "Group", "Column2", "Column3");

            ComplexHeaderCell[,] cells = this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 3));

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateRegularCell("Column1", rowSpan: 2),
                    this.CreateComplexCell("Group", columnSpan: 2),
                    null,
                },
                {
                    null,
                    this.CreateRegularCell("Column2"),
                    this.CreateRegularCell("Column3"),
                },
            });
        }

        /// <remarks>
        /// |---------------------------------------|
        /// | Column1 |            Group1           |
        /// |         |-----------------------------|
        /// |         |       Group2      | Column4 |
        /// |         |-------------------|         |
        /// |         | Column2 | Column3 |         |
        /// |---------------------------------------|
        /// </remarks>
        [Fact]
        public void AddGroupShouldAddMultipleComplexHeaderRows()
        {
            this.builder.AddGroup(0, "Group1", "Column2", "Column4");
            this.builder.AddGroup(1, "Group2", "Column2", "Column3");

            ComplexHeaderCell[,] cells = this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 4));

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateRegularCell("Column1", rowSpan: 3),
                    this.CreateComplexCell("Group1", columnSpan: 3),
                    null,
                    null,
                },
                {
                    null,
                    this.CreateComplexCell("Group2", columnSpan: 2),
                    null,
                    this.CreateRegularCell("Column4", rowSpan: 2),
                },
                {
                    null,
                    this.CreateRegularCell("Column2"),
                    this.CreateRegularCell("Column3"),
                    null,
                },
            });
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        public void AddGroupByColumnIndexesShouldThrowWhenColumnIndexIsLessThanZero(int startIndex, int endIndex)
        {
            Action action = () => this.builder.AddGroup(0, "Group", startIndex, endIndex);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public void AddGroupByColumnIndexesShouldThrowWhenColumnIndexIsGreaterThanLastColumn(int startIndex, int endIndex)
        {
            this.builder.AddGroup(0, "Group", startIndex, endIndex);

            Action action = () => this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 2));

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddGroupByColumnIndexesShouldThrowWhenRowIndexIsLessThanOne(int rowSpan)
        {
            Action action = () => this.builder.AddGroup(0, rowSpan, "Group", 0);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddGroupByColumnIndexesShouldThrowWhenRowSpanIsLessThanOne(int rowSpan)
        {
            Action action = () => this.builder.AddGroup(0, rowSpan, "Group", 0);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AddGroupByColumnIndexesShouldNotThrowWhenThereIsGapInRowNumbers()
        {
            this.builder.AddGroup(1, "Group1", 0, 1);
            this.builder.AddGroup(4, "Group2", 0, 1);

            ComplexHeaderCell[,] cells = this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 2));

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group1", columnSpan: 2),
                    null,
                },
                {
                    this.CreateComplexCell("Group2", columnSpan: 2),
                    null,
                },
                {
                    this.CreateRegularCell("Column1"),
                    this.CreateRegularCell("Column2"),
                },
            });
        }

        [Fact]
        public void AddGroupByColumnNamesShouldNotThrowWhenThereIsGapInRowNumbers()
        {
            this.builder.AddGroup(1, "Group1", "Column1", "Column2");
            this.builder.AddGroup(4, "Group2", "Column1", "Column2");

            ComplexHeaderCell[,] cells = this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 2));

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group1", columnSpan: 2),
                    null,
                },
                {
                    this.CreateComplexCell("Group2", columnSpan: 2),
                    null,
                },
                {
                    this.CreateRegularCell("Column1"),
                    this.CreateRegularCell("Column2"),
                },
            });
        }

        [Fact]
        public void AddGroupByColumnNamesShouldThrowWhenColumnNameIsInDifferentCase()
        {
            this.builder.AddGroup(0, "Group", "Column1".ToUpperInvariant());

            Action action = () => this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 1));

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void AddGroupByColumnNamesShouldThrowWhenColumnWithNameDoesNotExist()
        {
            this.builder.AddGroup(0, "Group", "ColumnX");

            Action action = () => this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 1));

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void AddGroupByColumnNamesShouldTakeFirstColumnWhenColumnNameIsDuplicated()
        {
            this.builder.AddGroup(0, "Group", "Column1", "Column3");

            ComplexHeaderCell[,] cells = this.builder.Build(new[] { "Column1", "Column2", "Column1", "Column3" });

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group", columnSpan: 4),
                    null,
                    null,
                    null,
                },
                {
                    this.CreateRegularCell("Column1"),
                    this.CreateRegularCell("Column2"),
                    this.CreateRegularCell("Column1"),
                    this.CreateRegularCell("Column3"),
                },
            });
        }

        [Theory]
        [InlineData(0, 3, 1, 2)]
        [InlineData(0, 2, 1, 3)]
        [InlineData(0, 2, 2, 3)]
        public void AddGroupByColumnIndexesShouldThrowWhenHeadersOverlapByColumn(int group1Start, int group1End, int group2Start, int group2End)
        {
            this.builder.AddGroup(0, "Group1", group1Start, group1End);
            this.builder.AddGroup(0, "Group2", group2Start, group2End);

            Action action = () => this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 4));

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Theory]
        [InlineData(0, 3, 1, 2)]
        [InlineData(0, 2, 1, 3)]
        [InlineData(0, 2, 2, 3)]
        public void AddGroupByColumnNamesShouldThrowWhenHeadersOverlapByColumn(int group1Start, int group1End, int group2Start, int group2End)
        {
            string[] columns = this.columnNames.Take(4).ToArray();
            this.builder.AddGroup(0, "Group1", columns[group1Start], columns[group1End]);
            this.builder.AddGroup(0, "Group2", columns[group2Start], columns[group2End]);

            Action action = () => this.builder.Build(columns);

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void AddGroupByColumnIndexesShouldThrowWhenHeadersOverlapByRow()
        {
            this.builder.AddGroup(0, 2, "Group1", 0, 2);
            this.builder.AddGroup(1, "Group2", 2);

            Action action = () => this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 3));

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void AddGroupByColumnNamesShouldThrowWhenHeadersOverlapByRow()
        {
            string[] columns = this.columnNames.Take(3).ToArray();
            this.builder.AddGroup(0, 2, "Group1", columns[0], columns[2]);
            this.builder.AddGroup(1, "Group2", columns[2]);

            Action action = () => this.builder.Build(columns);

            action.Should().ThrowExactly<ArgumentException>();
        }

        /// <remarks>
        /// Examples:
        /// Cells marked with "***" are not related to any group, so are in invalid state.
        ///
        /// |-----------------------------|
        /// | ******* |      Group1       |
        /// |-----------------------------|
        /// |      Group2       | Column3 |
        /// |-------------------|         |
        /// | Column1 | Column2 |         |
        /// |-----------------------------|
        ///
        /// |-------------------|
        /// | ******* | Group1  |
        /// |-------------------|
        /// |      Group2       |
        /// |-------------------|
        /// | Column1 | Column2 |
        /// |-------------------|
        ///
        /// |-------------------|
        /// | ******* | Group1  |
        /// |-------------------|
        /// | Group2  | ******* |
        /// |-------------------|
        /// | Column1 | Column2 |
        /// |-------------------|
        /// </remarks>
        [Theory]
        [InlineData(0, 3, 1, 2)]
        [InlineData(0, 2, 1, 3)]
        [InlineData(0, 2, 2, 3)]
        [InlineData(0, 1, 2, 3)]
        public void AddGroupByColumnIndexesShouldThrowWhenHigherRowGroupsSomeColumnsGroupedAtLowerRow(int lowerRowGroupStart, int lowerRowGroupEnd, int higherRowGroupStart, int higherRowGroupEnd)
        {
            this.builder.AddGroup(0, "Group1", higherRowGroupStart, higherRowGroupEnd);
            this.builder.AddGroup(1, "Group2", lowerRowGroupStart, lowerRowGroupEnd);

            Action action = () => this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 4));

            action.Should().ThrowExactly<ArgumentException>();
        }

        /// <see cref="AddGroupByColumnIndexesShouldThrowWhenHigherRowGroupsSomeColumnsGroupedAtLowerRow"/>
        [Theory]
        [InlineData(0, 3, 1, 2)]
        [InlineData(0, 2, 1, 3)]
        [InlineData(0, 2, 2, 3)]
        [InlineData(0, 1, 2, 3)]
        public void AddGroupByColumnNamesShouldThrowWhenHigherRowGroupsSomeColumnsGroupedAtLowerRow(int lowerRowGroupStart, int lowerRowGroupEnd, int higherRowGroupStart, int higherRowGroupEnd)
        {
            string[] columns = this.columnNames.Take(4).ToArray();
            this.builder.AddGroup(0, "Group1", columns[higherRowGroupStart], columns[higherRowGroupEnd]);
            this.builder.AddGroup(1, "Group2", columns[lowerRowGroupStart], columns[lowerRowGroupEnd]);

            Action action = () => this.builder.Build(columns);

            action.Should().ThrowExactly<ArgumentException>();
        }

        /// <remarks>
        /// Example:
        /// |-------------------|
        /// |         | Group2  |
        /// | Group1  |---------|
        /// |         | ******* |
        /// |-------------------|
        /// | Column1 | Column2 |
        /// |-------------------|
        ///
        /// Result will be:
        /// |-------------------|
        /// | Group1  | Group2  |
        /// |         |---------|
        /// |         | Column2 |
        /// |---------|         |
        /// | Column1 |         |
        /// |-------------------|
        /// </remarks>
        [Fact]
        public void AddGroupByColumnIndexesShouldSpanRowsWhenRegularCellIsNotGroupedAtLowerRows()
        {
            this.builder.AddGroup(0, 2, "Group1", 0);
            this.builder.AddGroup(0, "Group2", 1);

            ComplexHeaderCell[,] cells = this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 2));

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group1", rowSpan: 2),
                    this.CreateComplexCell("Group2"),
                },
                {
                    null,
                    this.CreateRegularCell("Column2", rowSpan: 2),
                },
                {
                    this.CreateRegularCell("Column1"),
                    null,
                },
            });
        }

        /// <see cref="AddGroupByColumnIndexesShouldSpanRowsWhenRegularCellIsNotGroupedAtLowerRows"/>
        [Fact]
        public void AddGroupByColumnNamesShouldSpanRowsWhenRegularCellIsNotGroupedAtLowerRows()
        {
            string[] columns = this.columnNames.Take(2).ToArray();
            this.builder.AddGroup(0, 2, "Group1", columns[0]);
            this.builder.AddGroup(0, "Group2", columns[1]);

            ComplexHeaderCell[,] cells = this.builder.Build(columns);

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group1", rowSpan: 2),
                    this.CreateComplexCell("Group2"),
                },
                {
                    null,
                    this.CreateRegularCell("Column2", rowSpan: 2),
                },
                {
                    this.CreateRegularCell("Column1"),
                    null,
                },
            });
        }

        /// <remarks>
        /// Example:
        /// |-----------------------------|
        /// |      Group1       | Group2  |
        /// |                   |---------|
        /// |                   | Group3  |
        /// |-----------------------------|
        /// | Column1 | Column2 | Column3 |
        /// |-----------------------------|
        /// </remarks>
        [Fact]
        public void AddGroupByColumnIndexesShouldBeAbleToSpanMultipleRowsAndColumns()
        {
            this.builder.AddGroup(0, 2, "Group1", 0, 1);
            this.builder.AddGroup(0, "Group2", 2, 2);
            this.builder.AddGroup(1, "Group3", 2, 2);

            ComplexHeaderCell[,] cells = this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 3));

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group1", columnSpan: 2, rowSpan: 2),
                    null,
                    this.CreateComplexCell("Group2"),
                },
                {
                    null,
                    null,
                    this.CreateComplexCell("Group3"),
                },
                {
                    this.CreateRegularCell("Column1"),
                    this.CreateRegularCell("Column2"),
                    this.CreateRegularCell("Column3"),
                },
            });
        }

        /// <see cref="AddGroupByColumnIndexesShouldBeAbleToSpanMultipleRowsAndColumns"/>
        [Fact]
        public void AddGroupByColumnNamesShouldBeAbleToSpanMultipleRowsAndColumns()
        {
            string[] columns = this.columnNames.Take(3).ToArray();
            this.builder.AddGroup(0, 2, "Group1", columns[0], columns[1]);
            this.builder.AddGroup(0, "Group2", columns[2], columns[2]);
            this.builder.AddGroup(1, "Group3", columns[2], columns[2]);

            ComplexHeaderCell[,] cells = this.builder.Build(columns);

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group1", columnSpan: 2, rowSpan: 2),
                    null,
                    this.CreateComplexCell("Group2"),
                },
                {
                    null,
                    null,
                    this.CreateComplexCell("Group3"),
                },
                {
                    this.CreateRegularCell("Column1"),
                    this.CreateRegularCell("Column2"),
                    this.CreateRegularCell("Column3"),
                },
            });
        }

        [Fact]
        public void AddGroupByColumnIndexesShouldSwapColumnsWhenStartColumnIsRightToEndColumn()
        {
            this.builder.AddGroup(0, "Group", 2, 0);

            ComplexHeaderCell[,] cells = this.builder.Build(new ArraySegment<string>(this.columnNames, 0, 3));

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group", columnSpan: 3),
                    null,
                    null,
                },
                {
                    this.CreateRegularCell("Column1"),
                    this.CreateRegularCell("Column2"),
                    this.CreateRegularCell("Column3"),
                },
            });
        }

        [Fact]
        public void AddGroupByColumnNamesShouldSwapColumnsWhenStartColumnIsRightToEndColumn()
        {
            string[] columns = this.columnNames.Take(3).ToArray();
            this.builder.AddGroup(0, "Group", columns[2], columns[0]);

            ComplexHeaderCell[,] cells = this.builder.Build(columns);

            cells.Should().BeEquivalentTo(new ComplexHeaderCell[,]
            {
                {
                    this.CreateComplexCell("Group", columnSpan: 3),
                    null,
                    null,
                },
                {
                    this.CreateRegularCell("Column1"),
                    this.CreateRegularCell("Column2"),
                    this.CreateRegularCell("Column3"),
                },
            });
        }

        private ComplexHeaderCell CreateComplexCell(string title, int columnSpan = 1, int rowSpan = 1)
        {
            return new ComplexHeaderCell()
            {
                Title = title,
                ColumnSpan = columnSpan,
                RowSpan = rowSpan,
                IsComplexHeaderCell = true,
            };
        }

        private ComplexHeaderCell CreateRegularCell(string title, int columnSpan = 1, int rowSpan = 1)
        {
            return new ComplexHeaderCell()
            {
                Title = title,
                ColumnSpan = columnSpan,
                RowSpan = rowSpan,
                IsComplexHeaderCell = false,
            };
        }
    }
}
