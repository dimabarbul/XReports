using System.Linq;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ComplexHeaderBuilderTest"/>
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
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, 2, "Group1", 0, 1);
            reportBuilder.AddComplexHeader(0, "Group2", 2, 3);
            reportBuilder.AddComplexHeader(1, "Group3", 2);
            reportBuilder.AddComplexHeader(1, "Group4", 3);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1")
                    {
                        ColumnSpan = 2,
                        RowSpan = 2,
                    },
                    null,
                    new ReportCellData("Group2")
                    {
                        ColumnSpan = 2,
                    },
                    null,
                },
                new object[]
                {
                    null,
                    null,
                    "Group3",
                    "Group4",
                },
                new object[]
                {
                    "Column1",
                    "Column2",
                    "Column3",
                    "Column4",
                },
            });
        }

        /// <see cref="AddComplexHeaderByColumnIndexesShouldAddCorrectHeader"/>
        [Fact]
        public void AddComplexHeaderByColumnNamesShouldAddCorrectHeader()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = this.CreateSchemaBuilder(4);
            reportBuilder.AddComplexHeader(0, 2, "Group1", "Column1", "Column2");
            reportBuilder.AddComplexHeader(0, "Group2", "Column3", "Column4");
            reportBuilder.AddComplexHeader(1, "Group3", "Column3");
            reportBuilder.AddComplexHeader(1, "Group4", "Column4");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1")
                    {
                        ColumnSpan = 2,
                        RowSpan = 2,
                    },
                    null,
                    new ReportCellData("Group2")
                    {
                        ColumnSpan = 2,
                    },
                    null,
                },
                new object[]
                {
                    null,
                    null,
                    "Group3",
                    "Group4",
                },
                new object[]
                {
                    "Column1",
                    "Column2",
                    "Column3",
                    "Column4",
                },
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
    }
}
