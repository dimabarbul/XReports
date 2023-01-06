using System;
using FluentAssertions;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.ReportSchemaCellsProviders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests
{
    public class AddPropertiesTest
    {
        [Fact]
        public void AddPropertiesShouldAddPropertiesToAllCells()
        {
            ReportSchemaCellsProviderBuilder<int> builder = new ReportSchemaCellsProviderBuilder<int>(
                "Value", new ComputedValueReportCellsProvider<int, int>(s => s));

            builder.AddProperties(new CustomProperty1(), new CustomProperty2());

            ReportCellProperty[] expectedProperties = { new CustomProperty1(), new CustomProperty2() };
            ReportSchemaCellsProvider<int> provider = builder.Build(ArraySegment<ReportCellProperty>.Empty);
            provider.CreateCell(0).Should().Be(new ReportCellData(0)
            {
                Properties = expectedProperties,
            });
            provider.CreateCell(1).Should().Be(new ReportCellData(1)
            {
                Properties = expectedProperties,
            });
        }

        [Fact]
        public void AddPropertiesShouldNotThrowWhenPropertyOfSameTypeAddedMultipleTimes()
        {
            ReportSchemaCellsProviderBuilder<int> builder = new ReportSchemaCellsProviderBuilder<int>(
                "Value", new ComputedValueReportCellsProvider<int, int>(s => s));

            builder.AddProperties(new CustomProperty1(), new CustomProperty1());

            ReportSchemaCellsProvider<int> provider = builder.Build(ArraySegment<ReportCellProperty>.Empty);
            ReportCellProperty[] expectedProperties = { new CustomProperty1(), new CustomProperty1() };
            provider.CreateCell(0).Should().Be(new ReportCellData(0)
            {
                Properties = expectedProperties,
            });
        }

        [Fact]
        public void AddPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            ReportSchemaCellsProviderBuilder<int> builder = new ReportSchemaCellsProviderBuilder<int>(
                "Value", new ComputedValueReportCellsProvider<int, int>(s => s));

            Action action = () => builder.AddProperties(new CustomProperty1(), new CustomProperty2(), null);

            action.Should().ThrowExactly<ArgumentException>();
        }

        private class CustomProperty1 : ReportCellProperty
        {
        }

        private class CustomProperty2 : ReportCellProperty
        {
        }
    }
}
