using System;
using System.Linq;
using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    public class AddComplexHeaderTest
    {
        [Fact]
        public void AddComplexHeaderByColumnIndexesShouldAddHeaderGroupingOneColumn()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(1);
            reportBuilder.AddComplexHeader(0, "Group", 0);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[] { 1 });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Group" },
                new[] { "Column1" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { 1 },
            });
        }

        [Fact]
        public void AddComplexHeaderByColumnNamesShouldAddHeaderGroupingOneColumn()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(1);
            reportBuilder.AddComplexHeader(0, "Group", "Column1");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[] { 1 });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Group" },
                new[] { "Column1" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { 1 },
            });
        }

        [Fact]
        public void AddComplexHeaderByColumnIndexesShouldAddHeaderGroupingMultipleColumns()
        {
            VerticalReportSchemaBuilder<int> reportBuilder =
                this.CreateSchemaBuilder(3);
            reportBuilder.AddComplexHeader(0, "Group", 0, 2);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group") { ColumnSpan = 3 },
                    null,
                    null,
                },
                new object[] { "Column1", "Column2", "Column3" },
            });
        }

        [Fact]
        public void AddComplexHeaderByColumnNamesShouldAddHeaderGroupingMultipleColumns()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(2);
            reportBuilder.AddComplexHeader(0, "Group", "Column1", "Column2");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group") { ColumnSpan = 2 },
                    null,
                },
                new object[] { "Column1", "Column2" },
            });
        }

        [Fact]
        public void AddComplexHeaderByColumnIndexesShouldAddHeaderWithMultipleGroupsInOneRow()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, "Group1", 0, 1);
            reportBuilder.AddComplexHeader(0, "Group2", 2, 3);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1") { ColumnSpan = 2 },
                    null,
                    new ReportCellData("Group2") { ColumnSpan = 2 },
                    null,
                },
                new object[] { "Column1", "Column2", "Column3", "Column4" },
            });
        }

        [Fact]
        public void AddComplexHeaderByColumnNamesShouldAddHeaderWithMultipleGroupsInOneRow()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(3);
            reportBuilder.AddComplexHeader(0, "Group1", "Column1", "Column2");
            reportBuilder.AddComplexHeader(0, "Group2", "Column3");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1") { ColumnSpan = 2 },
                    null,
                    "Group2",
                },
                new object[] { "Column1", "Column2", "Column3" },
            });
        }

        [Fact]
        public void AddComplexHeaderByColumnIndexesShouldAddHeaderNotGroupingAllColumns()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(3);
            reportBuilder.AddComplexHeader(0, "Group", 1, 2);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Column1") { RowSpan = 2 },
                    new ReportCellData("Group") { ColumnSpan = 2 },
                    null,
                },
                new object[] { null, "Column2", "Column3" },
            });
        }

        [Fact]
        public void AddComplexHeaderByColumnNamesShouldAddHeaderNotGroupingAllColumns()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(3);
            reportBuilder.AddComplexHeader(0, "Group", "Column2", "Column3");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Column1") { RowSpan = 2 },
                    new ReportCellData("Group") { ColumnSpan = 2 },
                    null,
                },
                new object[] { null, "Column2", "Column3" },
            });
        }

        [Fact]
        public void AddComplexHeaderShouldAddMultipleComplexHeaderRows()
        {
            /*
             |---------------------------------------|
             | Column1 |            Group1           |
             |         |-----------------------------|
             |         |       Group2      | Column4 |
             |         |-------------------|         |
             |         | Column2 | Column3 |         |
             |---------------------------------------|
             */
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, "Group1", "Column2", "Column4");
            reportBuilder.AddComplexHeader(1, "Group2", "Column2", "Column3");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Column1") { RowSpan = 3 },
                    new ReportCellData("Group1") { ColumnSpan = 3 },
                    null,
                    null,
                },
                new object[]
                {
                    null,
                    new ReportCellData("Group2") { ColumnSpan = 2 },
                    null,
                    new ReportCellData("Column4") { RowSpan = 2 },
                },
                new object[] { null, "Column2", "Column3", null },
            });
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        public void AddComplexHeaderByColumnIndexesShouldThrowWhenColumnIndexIsLessThanZero(int startIndex, int endIndex)
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(1);

            Action action = () => reportBuilder.AddComplexHeader(0, "Group", startIndex, endIndex);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public void AddComplexHeaderByColumnIndexesShouldThrowWhenColumnIndexIsGreaterThanLastColumn(int startIndex, int endIndex)
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(2);

            Action action = () => reportBuilder.AddComplexHeader(0, "Group", startIndex, endIndex);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AddComplexHeaderByColumnIndexesShouldThrowWhenRowIndexIsLessThanZero()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(1);

            Action action = () => reportBuilder.AddComplexHeader(-1, "Group", 0);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AddComplexHeaderByColumnIndexesShouldThrowWhenRowIndexIsGreaterThanNextRow()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(1);

            Action action = () => reportBuilder.AddComplexHeader(1, "Statistics", 0);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AddComplexHeaderByColumnNamesShouldThrowWhenColumnNameIsInDifferentCase()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(1);

            Action action = () => reportBuilder.AddComplexHeader(-1, "Group", "Column1".ToUpperInvariant());

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AddComplexHeaderByColumnNamesShouldThrowWhenColumnWithNameDoesNotExist()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(1);

            Action action = () => reportBuilder.AddComplexHeader(-1, "Group", "ColumnX");

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0, 3, 1, 2)]
        [InlineData(0, 2, 1, 3)]
        [InlineData(0, 2, 2, 3)]
        public void AddComplexHeaderByColumnIndexesShouldThrowWhenHeadersOverlap(int group1Start, int group1End, int group2Start, int group2End)
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, "Group1", group1Start, group1End);

            Action action = () => reportBuilder.AddComplexHeader(0, "Group2", group2Start, group2End);

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Theory]
        [InlineData(0, 3, 1, 2)]
        [InlineData(0, 2, 1, 3)]
        [InlineData(0, 2, 2, 3)]
        public void AddComplexHeaderByColumnNamesShouldThrowWhenHeadersOverlap(int group1Start, int group1End, int group2Start, int group2End)
        {
            string[] columns = { "Column1", "Column2", "Column3", "Column4" };
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(columns);

            reportBuilder.AddComplexHeader(0, "Group1", columns[group1Start], columns[group1End]);

            Action action = () =>
                reportBuilder.AddComplexHeader(0, "Group2", columns[group2Start], columns[group2End]);

            action.Should().ThrowExactly<ArgumentException>();
        }

        /// <remarks>
        /// Example:
        /// "Personal Info" groups "First Name" and "Last Name"
        /// "Employee" groups "Last Name" and "Age" (for example's sake)
        /// This leaves the most top left cell in an invalid state:
        /// it's not related to any of the groups.
        /// |------------------------------|
        /// | ********** |     Employee    |
        /// |------------------------------|
        /// |      Personal Info     | Age |
        /// |------------------------|     |
        /// | First Name | Last Name |     |
        /// |------------------------------|
        ///
        /// Another example:
        /// "Personal Info" groups "Name" on lower row
        /// "Job Info" groups "Salary" on higher row
        /// Cells marked with "***" are not related to any group,
        /// so are in invalid state.
        /// |--------------------------|
        /// | ************* | Job Info |
        /// |--------------------------|
        /// | Personal Info | ******** |
        /// |--------------------------|
        /// | Name          | Salary   |
        /// |--------------------------|
        /// </remarks>
        [Theory]
        [InlineData(0, 3, 1, 2)]
        [InlineData(0, 2, 1, 3)]
        [InlineData(0, 2, 2, 3)]
        public void AddComplexHeaderByColumnIndexesShouldThrowWhenHigherRowDoesNotGroupColumnGroupedAtLowerRow(int lowerRowGroupStart, int lowerRowGroupEnd, int higherRowGroupStart, int higherRowGroupEnd)
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, "Group1", higherRowGroupStart, higherRowGroupEnd);
            reportBuilder.AddComplexHeader(1, "Group2", lowerRowGroupStart, lowerRowGroupEnd);

            Action action = () => reportBuilder.BuildSchema();

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        /// <see cref="AddComplexHeaderByColumnIndexesShouldThrowWhenHigherRowDoesNotGroupColumnGroupedAtLowerRow"/>
        [Theory]
        [InlineData(0, 3, 1, 2)]
        [InlineData(0, 2, 1, 3)]
        [InlineData(0, 2, 2, 3)]
        public void AddComplexHeaderByColumnNamesShouldThrowWhenHigherRowDoesNotGroupColumnGroupedAtLowerRow(int lowerRowGroupStart, int lowerRowGroupEnd, int higherRowGroupStart, int higherRowGroupEnd)
        {
            string[] columns = { "Column1", "Column2", "Column3", "Column4" };
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(columns);
            reportBuilder.AddComplexHeader(0, "Group1", columns[higherRowGroupStart], columns[higherRowGroupEnd]);
            reportBuilder.AddComplexHeader(1, "Group2", columns[lowerRowGroupStart], columns[lowerRowGroupEnd]);

            Action action = () => reportBuilder.BuildSchema();

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void AddComplexHeaderByColumnIndexesShouldSwapColumnsWhenStartColumnIsRightToEndColumn()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(3);
            reportBuilder.AddComplexHeader(0, "Group", 2, 0);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group") { ColumnSpan = 3 },
                    null,
                    null,
                },
                new object[] { "Column1", "Column2", "Column3" },
            });
        }

        [Fact]
        public void AddComplexHeaderByColumnNamesShouldSwapColumnsWhenStartColumnIsRightToEndColumn()
        {
            string[] columns = { "Column1", "Column2", "Column3" };
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(columns);
            reportBuilder.AddComplexHeader(0, "Group", columns[2], columns[0]);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group") { ColumnSpan = 3 },
                    null,
                    null,
                },
                new object[] { "Column1", "Column2", "Column3" },
            });
        }

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(int columnsCount)
        {
            VerticalReportSchemaBuilder<int> reportBuilder =
                new VerticalReportSchemaBuilder<int>();

            for (int i = 0; i < columnsCount; i++)
            {
                reportBuilder.AddColumn($"Column{i + 1}", x => x);
            }

            return reportBuilder;
        }

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(params string[] columnNames)
        {
            VerticalReportSchemaBuilder<int> reportBuilder =
                new VerticalReportSchemaBuilder<int>();

            foreach (string column in columnNames)
            {
                reportBuilder.AddColumn(column, x => x);
            }

            return reportBuilder;
        }
    }
}
