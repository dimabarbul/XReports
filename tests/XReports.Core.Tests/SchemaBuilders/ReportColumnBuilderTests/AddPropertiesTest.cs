using System;
using FluentAssertions;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportColumnBuilderTests
{
    public class AddPropertiesTest
    {
        [Fact]
        public void AddPropertiesShouldAddPropertiesToAllCells()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellProvider<int, int>(s => s));

            builder.AddProperties(new CustomProperty1(), new CustomProperty2());

            ReportCellProperty[] expectedProperties = { new CustomProperty1(), new CustomProperty2() };
            IReportColumn<int> provider = builder.Build(Array.Empty<ReportCellProperty>());
            provider.CreateCell(0).Should().Equal(ReportCellHelper.CreateReportCell(0, expectedProperties));
            provider.CreateCell(1).Should().Equal(ReportCellHelper.CreateReportCell(1, expectedProperties));
        }

        [Fact]
        public void AddPropertiesShouldNotThrowWhenPropertyOfSameTypeAddedMultipleTimes()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellProvider<int, int>(s => s));

            builder.AddProperties(new CustomProperty1(), new CustomProperty1());

            IReportColumn<int> provider = builder.Build(Array.Empty<ReportCellProperty>());
            ReportCellProperty[] expectedProperties = { new CustomProperty1(), new CustomProperty1() };
            provider.CreateCell(0).Should().Equal(ReportCellHelper.CreateReportCell(0, expectedProperties));
        }

        [Fact]
        public void AddPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellProvider<int, int>(s => s));

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
