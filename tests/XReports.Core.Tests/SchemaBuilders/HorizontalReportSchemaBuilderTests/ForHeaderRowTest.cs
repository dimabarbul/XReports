using System;
using System.Linq;
using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    public class ForHeaderRowTest
    {
        [Fact]
        public void ForHeaderRowShouldSwitchContextToRowWithTheIndex()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder();
            schemaBuilder.AddHeaderRow("Header1", x => x);
            schemaBuilder.AddHeaderRow("Header2", x => x);

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForHeaderRow(0);

            cellsProviderBuilder.AddHeaderProperties(new CustomProperty());
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Header1", new CustomProperty()),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Header2"),
                },
            });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void ForHeaderRowShouldThrowWhenIndexIsOutOfRange(int index)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder();

            Action action = () => schemaBuilder.ForHeaderRow(index);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        private HorizontalReportSchemaBuilder<int> CreateSchemaBuilder()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();

            schemaBuilder.AddRow("Row", new EmptyCellsProvider<int>());

            return schemaBuilder;
        }

        private class CustomProperty : ReportCellProperty
        {
        }
    }
}
