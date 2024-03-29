using System;
using FluentAssertions;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.Table;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportColumnBuilderTests
{
    public class AddHeaderProcessorsTest
    {
        [Fact]
        public void AddHeaderProcessorsShouldAddProcessorsToBeCalledForHeaderCell()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "#", new ComputedValueReportCellProvider<int, int>(i => i));
            CustomHeaderCellProcessor1 processor1 = new CustomHeaderCellProcessor1();
            CustomHeaderCellProcessor2 processor2 = new CustomHeaderCellProcessor2();

            builder.AddHeaderProcessors(processor1, processor2);

            IReportColumn<int> provider = builder.Build(Array.Empty<IReportCellProperty>(), Array.Empty<IReportCellProcessor<int>>());
            provider.CreateHeaderCell();

            processor1.CallsCount.Should().Be(1);
            processor2.CallsCount.Should().Be(1);
        }

        [Fact]
        public void AddProcessorsShouldThrowWhenSomeProcessorIsNull()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "#", new ComputedValueReportCellProvider<int, int>(i => i));

            Action action = () => builder.AddHeaderProcessors(new CustomHeaderCellProcessor1(), new CustomHeaderCellProcessor2(), null);

            action.Should().ThrowExactly<ArgumentException>();
        }

        private abstract class CustomHeaderCellProcessor : IHeaderReportCellProcessor
        {
            public void Process(ReportCell cell)
            {
                this.CallsCount++;
            }

            public int CallsCount { get; private set; }
        }

        private class CustomHeaderCellProcessor1 : CustomHeaderCellProcessor
        {
        }

        private class CustomHeaderCellProcessor2 : CustomHeaderCellProcessor
        {
        }
    }
}
