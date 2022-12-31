using System;
using System.Globalization;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    public class AddGlobalPropertiesTest
    {
        [Fact]
        public void AddGlobalPropertiesShouldAddPropertiesToAllColumnsAndRows()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();
            schemaBuilder.AddColumn(new ComputedValueReportCellsProvider<int, int>("Value", x => x));
            schemaBuilder.AddColumn(new ComputedValueReportCellsProvider<int, string>("As string", x => x.ToString(CultureInfo.InvariantCulture)));

            schemaBuilder.AddGlobalProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(new[]
            {
                1,
                2,
            });
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] {"Value", "As string" },
            });
            ReportCellProperty[] expectedProperties =
            {
                new CustomProperty1(),
                new CustomProperty2(),
            };
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData(1)
                    {
                        Properties = expectedProperties,
                    },
                    new ReportCellData("1")
                    {
                        Properties = expectedProperties,
                    },
                },
                new object[]
                {
                    new ReportCellData(2)
                    {
                        Properties = expectedProperties,
                    },
                    new ReportCellData("2")
                    {
                        Properties = expectedProperties,
                    },
                },
            });
        }

        [Fact]
        public void AddGlobalPropertiesShouldAddOnlyPropertiesOfTypesNotAddedPreviously()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();
            schemaBuilder.AddColumn(new ComputedValueReportCellsProvider<int, int>("Value", x => x))
                .AddProperties(new CustomProperty1() { Value = true });
            schemaBuilder.AddColumn(new ComputedValueReportCellsProvider<int, string>("As string", x => x.ToString(CultureInfo.InvariantCulture)))
                .AddProperties(new CustomProperty2() { Value = true });

            schemaBuilder.AddGlobalProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(new[]
            {
                1,
            });
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] {"Value", "As string" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData(1)
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new CustomProperty1() { Value = true },
                            new CustomProperty2(),
                        },
                    },
                    new ReportCellData("1")
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new CustomProperty1(),
                            new CustomProperty2() { Value = true },
                        },
                    },
                },
            });
        }

        [Fact]
        public void AddGlobalPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();
            schemaBuilder.AddColumn(new ComputedValueReportCellsProvider<int, int>("Value", x => x));

            Action action = () => schemaBuilder.AddGlobalProperties(new CustomProperty1(), new CustomProperty2(), null);

            action.Should().ThrowExactly<ArgumentException>();
        }

        private class CustomProperty1 : ReportCellProperty
        {
            public bool Value { get; init; }
        }

        private class CustomProperty2 : ReportCellProperty
        {
            public bool Value { get; init; }
        }
    }
}
