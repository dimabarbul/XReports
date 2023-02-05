using System;
using System.Linq;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    public class ForRowTest
    {
        [Fact]
        public void ForRowByTitleShouldSwitchContextToRowWithTheTitle()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Row1", "Row2");

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForRow("Row1");

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Row1")
                    {
                        Properties = new [] { property },
                    },
                },
                new object[]
                {
                    "Row2",
                },
            });
        }

        [Fact]
        public void ForRowByTitleShouldThrowWhenTitleIsInDifferentCase()
        {
            const string rowName = "Row";
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(rowName);

            Action action = () => schemaBuilder.ForRow(rowName.ToUpperInvariant());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void ForRowByTitleShouldThrowWhenTitleDoesNotExist()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Row");

            Action action = () => schemaBuilder.ForRow("Row1");

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void ForRowByTitleShouldThrowWhenTitleIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Row");

            Action action = () => schemaBuilder.ForRow((string)null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void ForRowByTitleShouldSwitchContextToFirstOccurenceOfTitleWhenMultipleRowsHasTheTitle()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Row1", "Row2", "Row1");

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForRow("Row1");

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Row1")
                    {
                        Properties = new []{ property },
                    },
                },
                new object[]
                {
                    "Row2",
                },
                new object[]
                {
                    "Row1",
                },
            });
        }

        [Fact]
        public void ForRowByIndexShouldSwitchContextToRowWithTheIndex()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Row1", "Row2");

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForRow(0);

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Row1")
                    {
                        Properties = new [] { property },
                    },
                },
                new object[]
                {
                    "Row2",
                },
            });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void ForRowByIndexShouldThrowWhenIndexIsOutOfRange(int index)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Row1", "Row2");

            Action action = () => schemaBuilder.ForRow(index);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ForRowByIdShouldSwitchContextToColumnWithTheId()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder();
            schemaBuilder.AddRow(new RowId("1"), "Row1", new EmptyCellsProvider<int>());
            schemaBuilder.AddRow(new RowId("2"), "Row2", new EmptyCellsProvider<int>());

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForRow(new RowId("1"));

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Row1")
                    {
                        Properties = new [] { property },
                    },
                },
                new object[]
                {
                    "Row2",
                },
            });
        }

        [Fact]
        public void ForRowByIdShouldThrowWhenIdIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder();
            schemaBuilder.AddRow(new RowId("1"), "Row1", new EmptyCellsProvider<int>());
            schemaBuilder.AddRow(new RowId("2"), "Row2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.ForRow((RowId)null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void ForRowByIdShouldThrowWhenIdDoesNotExist()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder();
            schemaBuilder.AddRow(new RowId("1"), "Row1", new EmptyCellsProvider<int>());
            schemaBuilder.AddRow(new RowId("2"), "Row2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.ForRow(new RowId("3"));

            action.Should().ThrowExactly<ArgumentException>();
        }

        private HorizontalReportSchemaBuilder<int> CreateSchemaBuilder(params string[] rows)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();

            foreach (string row in rows)
            {
                schemaBuilder.AddRow(row, new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }

        private class CustomProperty : ReportCellProperty
        {
        }
    }
}
