using System;
using FluentAssertions;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellsProviders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportColumnBuilderTests
{
    public class AddGlobalPropertiesTest
    {
        [Fact]
        public void AddGlobalPropertiesShouldAddPropertiesToAllColumnsAndRows()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellsProvider<int, int>(x => x));

            IReportColumn<int> provider = builder.Build(
                new ReportCellProperty[] { new CustomProperty1(), new CustomProperty2() });

            ReportCellProperty[] expectedProperties =
            {
                new CustomProperty1(),
                new CustomProperty2(),
            };
            provider.CreateHeaderCell().Should().Equal(ReportCellHelper.CreateReportCell("Value"));
            provider.CreateCell(0).Should().Equal(ReportCellHelper.CreateReportCell(0, expectedProperties));
        }

        [Fact]
        public void AddGlobalPropertiesShouldAddOnlyPropertiesOfTypesNotAddedPreviously()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellsProvider<int, int>(x => x));

            builder.AddProperties(new CustomProperty1() { Value = true });
            IReportColumn<int> provider = builder.Build(
                new ReportCellProperty[] { new CustomProperty1(), new CustomProperty2() });

            ReportCellProperty[] expectedProperties =
            {
                new CustomProperty1() { Value = true },
                new CustomProperty2(),
            };
            provider.CreateHeaderCell().Should().Equal(ReportCellHelper.CreateReportCell("Value"));
            provider.CreateCell(0).Should().Equal(ReportCellHelper.CreateReportCell(0, expectedProperties));
        }

        [Fact]
        public void AddGlobalPropertiesShouldAddOnlyFirstPropertyOfTypeFromGlobalPropertiesList()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellsProvider<int, int>(x => x));

            IReportColumn<int> provider = builder.Build(
                new ReportCellProperty[] { new CustomProperty1(), new CustomProperty1() });

            ReportCellProperty[] expectedProperties =
            {
                new CustomProperty1(),
            };
            provider.CreateHeaderCell().Should().Equal(ReportCellHelper.CreateReportCell("Value"));
            provider.CreateCell(0).Should().Equal(ReportCellHelper.CreateReportCell(0, expectedProperties));
        }

        [Fact]
        public void AddGlobalPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellsProvider<int, int>(x => x));

            Action action = () => builder.Build(
                new ReportCellProperty[] { new CustomProperty1(), new CustomProperty2(), null });

            action.Should().ThrowExactly<ArgumentException>();
        }

        private class CustomProperty1 : ReportCellProperty
        {
            public bool Value { get; set; }
        }

        private class CustomProperty2 : ReportCellProperty
        {
        }
    }
}
