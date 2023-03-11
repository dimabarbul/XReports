using System;
using FluentAssertions;
using XReports.Schema;
using XReports.SchemaBuilder;
using XReports.SchemaBuilder.ReportCellsProviders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests
{
    public class AddHeaderPropertiesTest
    {
        [Fact]
        public void AddHeaderPropertiesShouldAddCustomHeaderProperties()
        {
            ReportColumnBuilder<string> builder = new ReportColumnBuilder<string>(
                "Value", new ComputedValueReportCellsProvider<string, string>(s => s));

            builder.AddHeaderProperties(new CustomHeaderProperty1(), new CustomHeaderProperty2());

            IReportColumn<string> provider = builder.Build(Array.Empty<ReportCellProperty>());
            ReportCell headerCell = provider.CreateHeaderCell();
            headerCell.Should().Equal(ReportCellHelper.CreateReportCell(
                "Value", new CustomHeaderProperty1(), new CustomHeaderProperty2()));
        }

        [Fact]
        public void AddHeaderPropertiesShouldNotThrowWhenPropertyOfSameTypeAddedMultipleTimes()
        {
            ReportColumnBuilder<string> builder = new ReportColumnBuilder<string>(
                "Value", new ComputedValueReportCellsProvider<string, string>(s => s));

            builder.AddHeaderProperties(new CustomHeaderProperty1(), new CustomHeaderProperty1());

            IReportColumn<string> provider = builder.Build(Array.Empty<ReportCellProperty>());
            ReportCell headerCell = provider.CreateHeaderCell();
            headerCell.Should().Equal(ReportCellHelper.CreateReportCell(
                "Value", new CustomHeaderProperty1(), new CustomHeaderProperty1()));
        }

        [Fact]
        public void AddHeaderPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            ReportColumnBuilder<string> builder = new ReportColumnBuilder<string>(
                "Value", new ComputedValueReportCellsProvider<string, string>(s => s));

            Action action = () => builder.AddHeaderProperties(
                new CustomHeaderProperty1(), new CustomHeaderProperty2(), null);

            action.Should().ThrowExactly<ArgumentException>();
        }

        private class CustomHeaderProperty1 : ReportCellProperty
        {
        }

        private class CustomHeaderProperty2 : ReportCellProperty
        {
        }
    }
}
