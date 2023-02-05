using System.Linq;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ComplexHeaderBuilderTest"/>
    public class AddComplexHeaderTest
    {
        /// <remarks>
        /// -------------------------------
        /// |      Group1       |  Row1   |
        /// |                   |---------|
        /// |                   |  Row2   |
        /// -------------------------------
        /// | Group2  | Group3  |  Row3   |
        /// |         | ------------------|
        /// |         | Group4  |  Row4   |
        /// -------------------------------
        /// </remarks>
        [Fact]
        public void AddComplexHeaderByRowIndexesShouldAddCorrectHeader()
        {
            HorizontalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, 2, "Group1", 0, 1);
            reportBuilder.AddComplexHeader(0, "Group2", 2, 3);
            reportBuilder.AddComplexHeader(1, "Group3", 2);
            reportBuilder.AddComplexHeader(1, "Group4", 3);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1")
                    {
                        RowSpan = 2,
                        ColumnSpan = 2,
                    },
                    null,
                    "Row1",
                },
                new object[]
                {
                    null,
                    null,
                    "Row2",
                },
                new object[]
                {
                    new ReportCellData("Group2")
                    {
                        RowSpan = 2,
                    },
                    "Group3",
                    "Row3",
                },
                new object[]
                {
                    null,
                    "Group4",
                    "Row4",
                },
            });
        }

        /// <see cref="AddComplexHeaderByRowIndexesShouldAddCorrectHeader"/>
        [Fact]
        public void AddComplexHeaderByRowNamesShouldAddCorrectHeader()
        {
            HorizontalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, 2, "Group1", "Row1", "Row2");
            reportBuilder.AddComplexHeader(0, "Group2", "Row3", "Row4");
            reportBuilder.AddComplexHeader(1, "Group3", "Row3");
            reportBuilder.AddComplexHeader(1, "Group4", "Row4");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1")
                    {
                        RowSpan = 2,
                        ColumnSpan = 2,
                    },
                    null,
                    "Row1",
                },
                new object[]
                {
                    null,
                    null,
                    "Row2",
                },
                new object[]
                {
                    new ReportCellData("Group2")
                    {
                        RowSpan = 2,
                    },
                    "Group3",
                    "Row3",
                },
                new object[]
                {
                    null,
                    "Group4",
                    "Row4",
                },
            });
        }

        /// <see cref="AddComplexHeaderByRowIndexesShouldAddCorrectHeader"/>
        [Fact]
        public void AddComplexHeaderByRowIdsShouldAddCorrectHeader()
        {
            HorizontalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(0);
            reportBuilder.AddRow(new RowId("1"), "Row1", new EmptyCellsProvider<int>());
            reportBuilder.AddRow(new RowId("2"), "Row2", new EmptyCellsProvider<int>());
            reportBuilder.AddRow(new RowId("3"), "Row3", new EmptyCellsProvider<int>());
            reportBuilder.AddRow(new RowId("4"), "Row4", new EmptyCellsProvider<int>());
            reportBuilder.AddComplexHeader(0, 2, "Group1", new RowId("1"), new RowId("2"));
            reportBuilder.AddComplexHeader(0, "Group2", new RowId("3"), new RowId("4"));
            reportBuilder.AddComplexHeader(1, "Group3", new RowId("3"));
            reportBuilder.AddComplexHeader(1, "Group4", new RowId("4"));

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1")
                    {
                        RowSpan = 2,
                        ColumnSpan = 2,
                    },
                    null,
                    "Row1",
                },
                new object[]
                {
                    null,
                    null,
                    "Row2",
                },
                new object[]
                {
                    new ReportCellData("Group2")
                    {
                        RowSpan = 2,
                    },
                    "Group3",
                    "Row3",
                },
                new object[]
                {
                    null,
                    "Group4",
                    "Row4",
                },
            });
        }

        private HorizontalReportSchemaBuilder<int> CreateSchemaBuilder(int rowsCount)
        {
            HorizontalReportSchemaBuilder<int> reportBuilder =
                new HorizontalReportSchemaBuilder<int>();

            for (int i = 0; i < rowsCount; i++)
            {
                reportBuilder.AddRow($"Row{i + 1}", x => x);
            }

            return reportBuilder;
        }
    }
}
