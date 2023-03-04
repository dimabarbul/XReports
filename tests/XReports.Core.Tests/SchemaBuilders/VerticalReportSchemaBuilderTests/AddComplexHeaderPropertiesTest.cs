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

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    public class AddComplexHeaderPropertiesTest
    {
        [Fact]
        public void AddComplexHeaderPropertiesShouldAddPropertiesToAllComplexHeaderCells()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(3);
            schemaBuilder.AddComplexHeader(0, "Group1", 0, 2);
            schemaBuilder.AddComplexHeader(1, "Group2", 0, 1);

            schemaBuilder.AddComplexHeaderProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            ReportCellProperty[] expectedProperties = new ReportCellProperty[]
            {
                new CustomProperty1(),
                new CustomProperty2(),
            };
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group1", columnSpan: 3, properties: expectedProperties),
                    null,
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group2", columnSpan: 2, properties: expectedProperties),
                    null,
                    ReportCellHelper.CreateReportCell("Column3", rowSpan: 2),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    null,
                },
            });
        }

        [Fact]
        public void AddComplexHeaderPropertiesShouldNotThrowWhenPropertyOfSameTypeAddedMultipleTimes()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(3);
            schemaBuilder.AddComplexHeader(0, "Group1", 0, 2);
            schemaBuilder.AddComplexHeader(1, "Group2", 0, 1);

            schemaBuilder.AddComplexHeaderProperties(new CustomProperty1(), new CustomProperty1());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            ReportCellProperty[] expectedProperties = new ReportCellProperty[]
            {
                new CustomProperty1(),
                new CustomProperty1(),
            };
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group1", columnSpan: 3, properties: expectedProperties),
                    null,
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group2", columnSpan: 2, properties: expectedProperties),
                    null,
                    ReportCellHelper.CreateReportCell("Column3", rowSpan: 2),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    null,
                },
            });
        }

        [Fact]
        public void AddComplexHeaderPropertiesWithTitleShouldAddPropertiesToAllComplexHeaderCellsWithTheTitleCaseSensitively()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            schemaBuilder.AddColumn("TheGroup", new EmptyCellsProvider<int>());
            schemaBuilder
                .AddComplexHeader(0, "TheGroup", 0, 2)
                .AddComplexHeader(1, "thegroup", 0, 1)
                .AddComplexHeader(1, "Group1", 2)
                .AddComplexHeader(2, "Group2", 0)
                .AddComplexHeader(2, "TheGroup", 1)
                .AddComplexHeader(2, "Group3", 2);

            schemaBuilder.AddComplexHeaderProperties("TheGroup", new CustomProperty1(), new CustomProperty2());
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            ReportCellProperty[] expectedProperties = new ReportCellProperty[]
            {
                new CustomProperty1(),
                new CustomProperty2(),
            };
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("TheGroup", columnSpan: 3, properties: expectedProperties),
                    null,
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("thegroup", columnSpan: 2),
                    null,
                    ReportCellHelper.CreateReportCell("Group1"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group2"),
                    ReportCellHelper.CreateReportCell("TheGroup", expectedProperties),
                    ReportCellHelper.CreateReportCell("Group3"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("TheGroup"),
                },
            });
        }

        [Fact]
        public void AddComplexHeaderPropertiesWithTitleShouldThrowWhenTitleIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(3);
            schemaBuilder.AddComplexHeader(0, "Group", 0, 2);

            Action action = () => schemaBuilder.AddComplexHeaderProperties(title: null, new CustomProperty1(), new CustomProperty2());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddComplexHeaderPropertiesWithTitleShouldIgnorePropertiesWhenTitleDoesNotExist()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(3);
            schemaBuilder.AddComplexHeader(0, "Group", 0, 2);

            schemaBuilder.AddComplexHeaderProperties("AnotherGroup", new CustomProperty1());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Group", columnSpan: 3),
                    null,
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("Column3"),
                },
            });
        }

        [Fact]
        public void AddComplexHeaderPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(3);
            schemaBuilder.AddComplexHeader(0, "Group", 0, 2);

            Action action = () => schemaBuilder.AddComplexHeaderProperties(new CustomProperty1(), new CustomProperty2(), null);

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

        private class CustomProperty1 : ReportCellProperty
        {
        }

        private class CustomProperty2 : ReportCellProperty
        {
        }
    }
}
