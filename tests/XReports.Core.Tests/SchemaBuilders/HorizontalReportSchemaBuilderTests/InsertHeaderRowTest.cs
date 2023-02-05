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

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    public class InsertHeaderRowTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InsertHeaderRowShouldInsertRowAtSpecifiedPosition(int index)
        {
            List<string> rows = new List<string>(new[] { "Header1", "Header2" });
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(rows);
            const string rowsName = "TheHeader";

            schemaBuilder.InsertHeaderRow(index, rowsName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            rows.Insert(index, rowsName);
            table.HeaderRows.Should().BeEquivalentTo(rows
                .Select(row => new object[] { row })
                .ToArray());
        }

        [Fact]
        public void InsertHeaderRowShouldInsertRowWithExistingTitle()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertHeaderRow(0, "Header0", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Header0" },
                new[] { "Header1" },
                new[] { "Header2" },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void InsertHeaderRowShouldInsertRowWithEmptyTitle(string title)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            schemaBuilder.InsertHeaderRow(0, title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { title },
            });
        }

        [Fact]
        public void InsertHeaderRowShouldThrowWhenTitleIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(1);

            Action action = () => schemaBuilder.InsertRow(0, null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertHeaderRowShouldThrowWhenIndexIsGreaterThanHeaderRowCount()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertRow(3, "Header", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void InsertHeaderRowShouldThrowWhenIndexIsLessThanZero()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertRow(-1, "Header", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        private HorizontalReportSchemaBuilder<int> CreateSchemaBuilder(int rowsCount)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();
            schemaBuilder.AddRow("Row", new EmptyCellsProvider<int>());

            for (int i = 0; i < rowsCount; i++)
            {
                schemaBuilder.AddHeaderRow($"Header{i + 1}", new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }

        private HorizontalReportSchemaBuilder<int> CreateSchemaBuilder(IEnumerable<string> headerRows)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();
            schemaBuilder.AddRow("Row", new EmptyCellsProvider<int>());

            foreach (string row in headerRows)
            {
                schemaBuilder.AddHeaderRow(row, new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }
    }
}
