using System;
using FluentAssertions;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.ReportSchemaCellsProviders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests
{
    public class AddGlobalPropertiesTest
    {
        [Fact]
        public void AddGlobalPropertiesShouldAddPropertiesToAllColumnsAndRows()
        {
            ReportSchemaCellsProviderBuilder<int> builder = new ReportSchemaCellsProviderBuilder<int>(
                "Value", new ComputedValueReportCellsProvider<int, int>(x => x));

            ReportSchemaCellsProvider<int> provider = builder.Build(
                new ReportCellProperty[] { new CustomProperty1(), new CustomProperty2() });

            ReportCellProperty[] expectedProperties =
            {
                new CustomProperty1(),
                new CustomProperty2(),
            };
            provider.CreateHeaderCell().Should().Be(new ReportCellData("Value"));
            provider.CreateCell(0).Should().Be(new ReportCellData(0)
            {
                Properties = expectedProperties,
            });
        }

        [Fact]
        public void AddGlobalPropertiesShouldAddOnlyPropertiesOfTypesNotAddedPreviously()
        {
            ReportSchemaCellsProviderBuilder<int> builder = new ReportSchemaCellsProviderBuilder<int>(
                "Value", new ComputedValueReportCellsProvider<int, int>(x => x));

            builder.AddProperties(new CustomProperty1() { Value = true });
            ReportSchemaCellsProvider<int> provider = builder.Build(
                new ReportCellProperty[] { new CustomProperty1(), new CustomProperty2() });

            ReportCellProperty[] expectedProperties =
            {
                new CustomProperty1() { Value = true },
                new CustomProperty2(),
            };
            provider.CreateHeaderCell().Should().Be(new ReportCellData("Value"));
            provider.CreateCell(0).Should().Be(new ReportCellData(0)
            {
                Properties = expectedProperties,
            });
        }

        [Fact]
        public void AddGlobalPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            ReportSchemaCellsProviderBuilder<int> builder = new ReportSchemaCellsProviderBuilder<int>(
                "Value", new ComputedValueReportCellsProvider<int, int>(x => x));

            Action action = () => builder.Build(
                new ReportCellProperty[] { new CustomProperty1(), new CustomProperty2(), null });

            action.Should().ThrowExactly<ArgumentException>();
        }

        private class CustomProperty1 : ReportCellProperty
        {
            public bool Value { get; init; }
        }

        private class CustomProperty2 : ReportCellProperty
        {
        }
    }
}
