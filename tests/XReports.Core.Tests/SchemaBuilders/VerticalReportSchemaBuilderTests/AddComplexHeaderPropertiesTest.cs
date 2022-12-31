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
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1")
                    {
                        ColumnSpan = 3,
                        Properties = expectedProperties,
                    },
                    null,
                    null,
                },
                new object[]
                {
                    new ReportCellData("Group2")
                    {
                        ColumnSpan = 2,
                        Properties = expectedProperties,
                    },
                    null,
                    new ReportCellData("Column3")
                    {
                        RowSpan = 2,
                    },
                },
                new object[]
                {
                    "Column1",
                    "Column2",
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
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group1")
                    {
                        ColumnSpan = 3,
                        Properties = expectedProperties,
                    },
                    null,
                    null,
                },
                new object[]
                {
                    new ReportCellData("Group2")
                    {
                        ColumnSpan = 2,
                        Properties = expectedProperties,
                    },
                    null,
                    new ReportCellData("Column3")
                    {
                        RowSpan = 2,
                    },
                },
                new object[]
                {
                    "Column1",
                    "Column2",
                    null,
                },
            });
        }

        [Fact]
        public void AddComplexHeaderPropertiesWithTitleShouldAddPropertiesToAllComplexHeaderCellsWithTheTitleCaseSensitively()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            schemaBuilder.AddColumn(new EmptyCellsProvider<int>("TheGroup"));
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
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("TheGroup")
                    {
                        ColumnSpan = 3,
                        Properties = expectedProperties,
                    },
                    null,
                    null,
                },
                new object[]
                {
                    new ReportCellData("thegroup")
                    {
                        ColumnSpan = 2,
                    },
                    null,
                    "Group1",
                },
                new object[]
                {
                    "Group2",
                    new ReportCellData("TheGroup")
                    {
                        Properties = expectedProperties,
                    },
                    "Group3",
                },
                new object[]
                {
                    "Column1",
                    "Column2",
                    "TheGroup",
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
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Group")
                    {
                        ColumnSpan = 3,
                    },
                    null,
                    null,
                },
                new object[]
                {
                    "Column1",
                    "Column2",
                    "Column3",
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
                schemaBuilder.AddColumn(new EmptyCellsProvider<int>($"Column{i + 1}"));
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
