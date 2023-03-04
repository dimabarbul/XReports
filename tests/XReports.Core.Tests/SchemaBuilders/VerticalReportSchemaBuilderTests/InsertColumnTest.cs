using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
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

            schemaBuilder.InsertColumn(index, columnName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            columns.Insert(index, columnName);
            table.HeaderRows.Should().Equal(new[]
            {
                columns.Select(c => ReportCellHelper.CreateReportCell(c)).ToArray(),
            });
        }

        [Fact]
        public void InsertColumnShouldInsertColumnWithExistingTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumn(0, "Column2", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void InsertColumnShouldInsertColumnWithEmptyTitle(string title)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            schemaBuilder.InsertColumn(0, title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(title),
                },
            });
        }

        [Fact]
        public void InsertColumnShouldThrowWhenTitleIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(1);

            Action action = () => schemaBuilder.InsertColumn(0, null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnShouldThrowWhenIndexIsGreaterThanColumnCount()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumn(3, "Column", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void InsertColumnShouldThrowWhenIndexIsLessThanZero()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumn(-1, "Column", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void InsertColumnShouldInsertColumnWithId()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(new[] { "Column1", "Column2" });

            schemaBuilder.InsertColumn(0, new ColumnId("Column"), "TheColumn", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("TheColumn"),
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                },
            });
        }

        [Fact]
        public void InsertColumnShouldThrowWhenIdIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumn(0, null, "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnShouldThrowWhenIdExists()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("Column"), "Column1", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertColumn(0, new ColumnId("Column"), "Column2", new EmptyCellsProvider<int>());

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

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(IEnumerable<string> columns)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();

            foreach (string column in columns)
            {
                schemaBuilder.AddColumn(column, new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }
    }
}
