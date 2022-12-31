using System;
using System.Collections.Generic;
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
    public class InsertColumnTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InsertColumnShouldInsertColumnAtSpecifiedPosition(int index)
        {
            List<string> columns = new List<string>(new[] { "Column1", "Column2" });
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(columns);
            const string columnName = "TheColumn";

            schemaBuilder.InsertColumn(index, new EmptyCellsProvider<int>(columnName));

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            columns.Insert(index, columnName);
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                columns,
            });
        }

        [Fact]
        public void InsertColumnShouldInsertColumnWithExistingTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumn(0, new EmptyCellsProvider<int>("Column2"));

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { "Column2", "Column1", "Column2" },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void InsertColumnShouldInsertColumnWithEmptyTitle(string title)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            schemaBuilder.InsertColumn(0, new EmptyCellsProvider<int>(title));

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { title },
            });
        }

        [Fact]
        public void InsertColumnShouldThrowWhenTitleIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(1);

            Action action = () => schemaBuilder.InsertColumn(0, new EmptyCellsProvider<int>(null));

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnShouldThrowWhenIndexIsGreaterThanColumnCount()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumn(3, new EmptyCellsProvider<int>("Column"));

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void InsertColumnShouldThrowWhenIndexIsLessThanZero()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumn(-1, new EmptyCellsProvider<int>("Column"));

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(int columnsCount)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();

            for (int i = 0; i < columnsCount; i++)
            {
                schemaBuilder.AddColumn(new EmptyCellsProvider<int>($"Column{i + 1}"));
            }

            return schemaBuilder;
        }

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(IEnumerable<string> columns)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();

            foreach (string column in columns)
            {
                schemaBuilder.AddColumn(new EmptyCellsProvider<int>(column));
            }

            return schemaBuilder;
        }
    }
}
