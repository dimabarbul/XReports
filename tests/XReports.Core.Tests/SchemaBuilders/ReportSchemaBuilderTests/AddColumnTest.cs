using System;
using System.Linq;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    public class AddColumnTest
    {
        [Fact]
        public void AddColumnShouldAddColumnAtEnd()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            const string columnName = "TheColumn";

            schemaBuilder.AddColumn(columnName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell(columnName),
                },
            });
        }

        [Fact]
        public void AddColumnShouldAddColumnAtEndForHorizontal()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            const string columnName = "TheColumn";

            schemaBuilder.AddColumn(columnName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildHorizontalSchema(0).BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column2"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell(columnName),
                },
            });
        }

        [Fact]
        public void AddColumnShouldAddColumnWithExistingTitle()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.AddColumn("Column1", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("Column1"),
                },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void AddColumnShouldAddColumnWithEmptyTitle(string title)
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            schemaBuilder.AddColumn(title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(title),
                },
            });
        }

        [Fact]
        public void AddColumnShouldThrowWhenTitleIsNull()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            Action action = () => schemaBuilder.AddColumn(null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddColumnShouldAddColumnWithId()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            ColumnId id = new ColumnId("Column");
            const string columnName = "TheColumn";

            schemaBuilder.AddColumn(id, columnName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell(columnName),
                },
            });
        }

        [Fact]
        public void AddColumnShouldThrowWhenIdIsNull()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.AddColumn(null, "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddColumnShouldThrowWhenIdExists()
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();
            ColumnId id = new ColumnId("Column");
            schemaBuilder.AddColumn(id, "Column1", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.AddColumn(id, "Column2", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        private ReportSchemaBuilder<int> CreateSchemaBuilder(int columnsCount)
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();

            for (int i = 0; i < columnsCount; i++)
            {
                schemaBuilder.AddColumn($"Column{i + 1}", new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }
    }
}
