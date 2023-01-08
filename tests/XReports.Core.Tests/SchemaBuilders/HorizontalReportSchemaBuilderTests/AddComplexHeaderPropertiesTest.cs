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
    public class AddComplexHeaderPropertiesTest
    {
        [Fact]
        public void AddComplexHeaderPropertiesShouldAddPropertiesToAllComplexHeaderCells()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(3);
            schemaBuilder.AddComplexHeader(0, "Group1", 0, 2);
            schemaBuilder.AddComplexHeader(1, "Group2", 0, 1);

            schemaBuilder.AddComplexHeaderProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            ReportCellProperty[] expectedProperties = new ReportCellProperty[]
            {
                new CustomProperty1(),
                new CustomProperty2(),
            };
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1")
                    {
                        RowSpan = 3,
                        Properties = expectedProperties,
                    },
                    new ReportCellData("Group2")
                    {
                        RowSpan = 2,
                        Properties = expectedProperties,
                    },
                    "Row1",
                },
                new object[]
                {
                    null,
                    null,
                    "Row2",
                },
                new object[]
                {
                    null,
                    new ReportCellData("Row3")
                    {
                        ColumnSpan = 2,
                    },
                    null,
                },
            });
        }

        [Fact]
        public void AddComplexHeaderPropertiesShouldNotThrowWhenPropertyOfSameTypeAddedMultipleTimes()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(3);
            schemaBuilder.AddComplexHeader(0, "Group1", 0, 2);
            schemaBuilder.AddComplexHeader(1, "Group2", 0, 1);

            schemaBuilder.AddComplexHeaderProperties(new CustomProperty1(), new CustomProperty1());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            ReportCellProperty[] expectedProperties = new ReportCellProperty[]
            {
                new CustomProperty1(),
                new CustomProperty1(),
            };
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1")
                    {
                        RowSpan = 3,
                        Properties = expectedProperties,
                    },
                    new ReportCellData("Group2")
                    {
                        RowSpan = 2,
                        Properties = expectedProperties,
                    },
                    "Row1",
                },
                new object[]
                {
                    null,
                    null,
                    "Row2",
                },
                new object[]
                {
                    null,
                    new ReportCellData("Row3")
                    {
                        ColumnSpan = 2,
                    },
                    null,
                },
            });
        }

        [Fact]
        public void AddComplexHeaderPropertiesWithTitleShouldAddPropertiesToAllComplexHeaderCellsWithTheTitleCaseSensitively()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            schemaBuilder.AddRow("TheGroup", new EmptyCellsProvider<int>());
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
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("TheGroup")
                    {
                        RowSpan = 3,
                        Properties = expectedProperties,
                    },
                    new ReportCellData("thegroup")
                    {
                        RowSpan = 2,
                    },
                    "Group2",
                    "Row1",
                },
                new object[]
                {
                    null,
                    null,
                    new ReportCellData("TheGroup")
                    {
                        Properties = expectedProperties,
                    },
                    "Row2",
                },
                new object[]
                {
                    null,
                    "Group1",
                    "Group3",
                    "TheGroup",
                },
            });
        }

        [Fact]
        public void AddComplexHeaderPropertiesWithTitleShouldThrowWhenTitleIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(3);
            schemaBuilder.AddComplexHeader(0, "Group", 0, 2);

            Action action = () => schemaBuilder.AddComplexHeaderProperties(title: null, new CustomProperty1(), new CustomProperty2());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddComplexHeaderPropertiesWithTitleShouldIgnorePropertiesWhenTitleDoesNotExist()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(3);
            schemaBuilder.AddComplexHeader(0, "Group", 0, 2);

            schemaBuilder.AddComplexHeaderProperties("AnotherGroup", new CustomProperty1());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group")
                    {
                        RowSpan = 3,
                    },
                    "Row1",
                },
                new object[]
                {
                    null,
                    "Row2",
                },
                new object[]
                {
                    null,
                    "Row3",
                },
            });
        }

        [Fact]
        public void AddComplexHeaderPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(3);
            schemaBuilder.AddComplexHeader(0, "Group", 0, 2);

            Action action = () => schemaBuilder.AddComplexHeaderProperties(new CustomProperty1(), new CustomProperty2(), null);

            action.Should().ThrowExactly<ArgumentException>();
        }

        private HorizontalReportSchemaBuilder<int> CreateSchemaBuilder(int columnsCount)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();

            for (int i = 0; i < columnsCount; i++)
            {
                schemaBuilder.AddRow($"Row{i + 1}", new EmptyCellsProvider<int>());
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
