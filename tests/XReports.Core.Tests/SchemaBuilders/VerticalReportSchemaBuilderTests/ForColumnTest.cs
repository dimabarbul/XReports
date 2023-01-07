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
    public class ForColumnTest
    {
        [Fact]
        public void ForColumnByTitleShouldSwitchContextToColumnWithTheTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column1", "Column2");

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForColumn("Column1");

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Column1")
                    {
                        Properties = new [] { property },
                    },
                    "Column2",
                },
            });
        }

        [Fact]
        public void ForColumnByTitleShouldThrowWhenTitleIsInDifferentCase()
        {
            const string columnName = "Column";
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(columnName);

            Action action = () => schemaBuilder.ForColumn(columnName.ToUpperInvariant());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void ForColumnByTitleShouldThrowWhenTitleDoesNotExist()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column");

            Action action = () => schemaBuilder.ForColumn("Column1");

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void ForColumnByTitleShouldThrowWhenTitleIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column");

            Action action = () => schemaBuilder.ForColumn(null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void ForColumnByTitleShouldSwitchContextToFirstOccurenceOfTitleWhenMultipleColumnsHasTheTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column1", "Column2", "Column1");

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForColumn("Column1");

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Column1")
                    {
                        Properties = new []{ property },
                    },
                    "Column2",
                    "Column1",
                },
            });
        }

        [Fact]
        public void ForColumnByIndexShouldSwitchContextToColumnWithTheIndex()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column1", "Column2");

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForColumn(0);

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Column1")
                    {
                        Properties = new [] { property },
                    },
                    "Column2",
                },
            });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void ForColumnByIndexShouldThrowWhenIndexIsOutOfRange(int index)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column1", "Column2");

            Action action = () => schemaBuilder.ForColumn(index);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(params string[] columns)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();

            foreach (string column in columns)
            {
                schemaBuilder.AddColumn(column, new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }

        private class CustomProperty : ReportCellProperty
        {
        }
    }
}
