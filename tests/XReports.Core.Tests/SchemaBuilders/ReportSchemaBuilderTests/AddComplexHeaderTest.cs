using System.Linq;
using XReports.Core.Tests.ComplexHeader;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    /// <seealso cref="ComplexHeaderBuilderTest"/>
    public class AddComplexHeaderTest
    {
        /// <remarks>
        /// -----------------------------------------
        /// |      Group1       |       Group2      |
        /// |                   |-------------------|
        /// |                   | Group3  | Group4  |
        /// -----------------------------------------
        /// | Column1 | Column2 | Column3 | Column4 |
        /// -----------------------------------------
        /// </remarks>
        [Fact]
        public void AddComplexHeaderByColumnIndexesShouldAddCorrectHeader()
        {
            ReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, 2, "Group1", 0, 1);
            reportBuilder.AddComplexHeader(0, "Group2", 2, 3);
            reportBuilder.AddComplexHeader(1, "Group3", 2);
            reportBuilder.AddComplexHeader(1, "Group4", 3);

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group1", columnSpan: 2, rowSpan: 2),
                    null,
                    ReportCellHelper.CreateReportCell("Group2", columnSpan: 2),
                    null,
                },
                new[]
                {
                    null,
                    null,
                    ReportCellHelper.CreateReportCell("Group3"),
                    ReportCellHelper.CreateReportCell("Group4"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("Column3"),
                    ReportCellHelper.CreateReportCell("Column4"),
                },
            });
        }

        /// <see cref="AddComplexHeaderByColumnIndexesShouldAddCorrectHeader"/>
        [Fact]
        public void AddComplexHeaderByColumnIndexesShouldAddCorrectHeaderForHorizontal()
        {
            ReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, 2, "Group1", 0, 1);
            reportBuilder.AddComplexHeader(0, "Group2", 2, 3);
            reportBuilder.AddComplexHeader(1, "Group3", 2);
            reportBuilder.AddComplexHeader(1, "Group4", 3);

            IReportTable<ReportCell> table = reportBuilder.BuildHorizontalSchema(0).BuildReportTable(Enumerable.Empty<int>());

            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group1", rowSpan: 2, columnSpan: 2),
                    null,
                    ReportCellHelper.CreateReportCell("Column1"),
                },
                new[]
                {
                    null,
                    null,
                    ReportCellHelper.CreateReportCell("Column2"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group2", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Group3"),
                    ReportCellHelper.CreateReportCell("Column3"),
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Group4"),
                    ReportCellHelper.CreateReportCell("Column4"),
                },
            });
        }

        /// <see cref="AddComplexHeaderByColumnIndexesShouldAddCorrectHeader"/>
        [Fact]
        public void AddComplexHeaderByColumnNamesShouldAddCorrectHeader()
        {
            ReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, 2, "Group1", "Column1", "Column2");
            reportBuilder.AddComplexHeader(0, "Group2", "Column3", "Column4");
            reportBuilder.AddComplexHeader(1, "Group3", "Column3");
            reportBuilder.AddComplexHeader(1, "Group4", "Column4");

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group1", columnSpan: 2, rowSpan: 2),
                    null,
                    ReportCellHelper.CreateReportCell("Group2", columnSpan: 2),
                    null,
                },
                new[]
                {
                    null,
                    null,
                    ReportCellHelper.CreateReportCell("Group3"),
                    ReportCellHelper.CreateReportCell("Group4"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("Column3"),
                    ReportCellHelper.CreateReportCell("Column4"),
                },
            });
        }

        /// <see cref="AddComplexHeaderByColumnIndexesShouldAddCorrectHeader"/>
        [Fact]
        public void AddComplexHeaderByColumnIdsShouldAddCorrectHeader()
        {
            ReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(0);
            reportBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellsProvider<int>());
            reportBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellsProvider<int>());
            reportBuilder.AddColumn(new ColumnId("3"), "Column3", new EmptyCellsProvider<int>());
            reportBuilder.AddColumn(new ColumnId("4"), "Column4", new EmptyCellsProvider<int>());
            reportBuilder.AddComplexHeader(0, 2, "Group1", new ColumnId("1"), new ColumnId("2"));
            reportBuilder.AddComplexHeader(0, "Group2", new ColumnId("3"), new ColumnId("4"));
            reportBuilder.AddComplexHeader(1, "Group3", new ColumnId("3"));
            reportBuilder.AddComplexHeader(1, "Group4", new ColumnId("4"));

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group1", columnSpan: 2, rowSpan: 2),
                    null,
                    ReportCellHelper.CreateReportCell("Group2", columnSpan: 2),
                    null,
                },
                new[]
                {
                    null,
                    null,
                    ReportCellHelper.CreateReportCell("Group3"),
                    ReportCellHelper.CreateReportCell("Group4"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("Column3"),
                    ReportCellHelper.CreateReportCell("Column4"),
                },
            });
        }

        private ReportSchemaBuilder<int> CreateSchemaBuilder(int columnsCount)
        {
            ReportSchemaBuilder<int> reportBuilder =
                new ReportSchemaBuilder<int>();

            for (int i = 0; i < columnsCount; i++)
            {
                reportBuilder.AddColumn($"Column{i + 1}", x => x);
            }

            return reportBuilder;
        }
    }
}
