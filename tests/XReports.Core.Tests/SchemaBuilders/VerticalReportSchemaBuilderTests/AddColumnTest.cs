using System;
using System.Linq;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    public class AddColumnTest
    {
        [Fact]
        public void AddColumnShouldAddColumnAtEnd()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            const string columnName = "TheColumn";

            schemaBuilder.AddColumn(columnName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { "Column1", "Column2", "TheColumn" },
            });
        }

        [Fact]
        public void AddColumnShouldAddColumnWithExistingTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.AddColumn("Column1", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { "Column1", "Column2", "Column1" },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void AddColumnShouldAddColumnWithEmptyTitle(string title)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            schemaBuilder.AddColumn(title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { title },
            });
        }

        [Fact]
        public void AddColumnShouldThrowWhenTitleIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            Action action = () => schemaBuilder.AddColumn(null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(int columnsCount)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();

            for (int i = 0; i < columnsCount; i++)
            {
                schemaBuilder.AddColumn($"Column{i + 1}", new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }
    }
}
