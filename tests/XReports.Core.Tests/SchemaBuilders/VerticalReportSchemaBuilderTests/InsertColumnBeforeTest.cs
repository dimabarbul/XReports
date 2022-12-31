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
    public class InsertColumnBeforeTest
    {
        [Fact]
        public void InsertColumnBeforeShouldInsertColumnAtCorrectPosition()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column1", new EmptyCellsProvider<int>("TheColumn"));

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "TheColumn", "Column1", "Column2" },
            });
        }

        [Fact]
        public void InsertColumnBeforeShouldInsertColumnWithExistingTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column2", new EmptyCellsProvider<int>("Column1"));

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Column1", "Column1", "Column2" },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void InsertColumnBeforeShouldInsertColumnWithEmptyTitle(string title)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column1", new EmptyCellsProvider<int>(title));

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { title, "Column1", "Column2" },
            });
        }

        [Fact]
        public void InsertColumnBeforeShouldThrowWhenTitleIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore("Column1", new EmptyCellsProvider<int>(null));

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeShouldThrowWhenBeforeColumnNameIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore(null, new EmptyCellsProvider<int>("TheColumn"));

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeShouldThrowWhenTitleIsInDifferentCase()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore("Column1".ToUpperInvariant(), new EmptyCellsProvider<int>("TheColumn"));

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeShouldThrowWhenTitleDoesNotExist()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore("ColumnX".ToUpperInvariant(), new EmptyCellsProvider<int>("TheColumn"));

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeShouldInsertColumnBeforeFirstOccurenceOfTitleWhenMultipleColumnsHasTheTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(new[] { "Column1", "Column2", "Column1" });

            schemaBuilder.InsertColumnBefore("Column1", new EmptyCellsProvider<int>("TheColumn"));

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "TheColumn", "Column1", "Column2", "Column1" },
            });
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
