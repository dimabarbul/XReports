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
    public class AddGlobalPropertiesTest
    {
        [Fact]
        public void AddGlobalPropertiesShouldAddPropertiesToAllColumnsAndRows()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellProvider<int, int>(x => x));

            IReportColumn<int> provider = builder.Build(
                new IReportCellProperty[] { new CustomProperty1(), new CustomProperty2() },
                Array.Empty<IReportCellProcessor<int>>());

            IReportCellProperty[] expectedProperties =
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
                "Value", new ComputedValueReportCellProvider<int, int>(x => x));

            builder.AddProperties(new CustomProperty1() { Value = true });
            IReportColumn<int> provider = builder.Build(
                new IReportCellProperty[] { new CustomProperty1(), new CustomProperty2() },
                Array.Empty<IReportCellProcessor<int>>());

            IReportCellProperty[] expectedProperties =
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
                "Value", new ComputedValueReportCellProvider<int, int>(x => x));

            IReportColumn<int> provider = builder.Build(
                new IReportCellProperty[] { new CustomProperty1(), new CustomProperty1() },
                Array.Empty<IReportCellProcessor<int>>());

            IReportCellProperty[] expectedProperties =
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
                "Value", new ComputedValueReportCellProvider<int, int>(x => x));

            Action action = () => builder.Build(
                new IReportCellProperty[] { new CustomProperty1(), new CustomProperty2(), null },
                Array.Empty<IReportCellProcessor<int>>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        private class CustomProperty1 : IReportCellProperty
        {
            public bool Value { get; set; }
        }

        private class CustomProperty2 : IReportCellProperty
        {
        }
    }
}
