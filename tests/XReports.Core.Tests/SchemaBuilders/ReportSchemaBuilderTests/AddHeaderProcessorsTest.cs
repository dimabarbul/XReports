using System.Linq;
using FluentAssertions;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    /// <seealso cref="ReportColumnBuilderTests.AddHeaderProcessorsTest"/>
    public class AddHeaderProcessorsTest
    {
        [Fact]
        public void AddHeaderProcessorsShouldAddProcessorsToBeCalledForHeaderCell()
        {
            ReportSchemaBuilder<int> reportBuilder = new ReportSchemaBuilder<int>();
            CustomHeaderCellProcessor1 processor1 = new CustomHeaderCellProcessor1();
            CustomHeaderCellProcessor2 processor2 = new CustomHeaderCellProcessor2();
            reportBuilder.AddColumn("#", i => i)
                .AddHeaderProcessors(processor1, processor2);

            IReportTable<ReportCell> _ = reportBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());

            processor1.CallsCount.Should().Be(1);
            processor2.CallsCount.Should().Be(1);
        }

        [Fact]
        public void AddHeaderProcessorsShouldAddProcessorsToBeCalledForHeaderCellForHorizontal()
        {
            ReportSchemaBuilder<int> reportBuilder = new ReportSchemaBuilder<int>();
            CustomHeaderCellProcessor1 processor1 = new CustomHeaderCellProcessor1();
            CustomHeaderCellProcessor2 processor2 = new CustomHeaderCellProcessor2();
            reportBuilder.AddColumn("#", i => i)
                .AddHeaderProcessors(processor1, processor2);

            IReportTable<ReportCell> _ = reportBuilder.BuildHorizontalSchema(0).BuildReportTable(Enumerable.Empty<int>());

            processor1.CallsCount.Should().Be(1);
            processor2.CallsCount.Should().Be(1);
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
