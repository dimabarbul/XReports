using System.Linq;
using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddHeaderProcessorsTest"/>
    public class AddHeaderProcessorsTest
    {
        [Fact]
        public void AddHeaderProcessorsShouldAddProcessorsToBeCalledForHeaderCell()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = new VerticalReportSchemaBuilder<int>();
            CustomHeaderCellProcessor1 processor1 = new CustomHeaderCellProcessor1();
            CustomHeaderCellProcessor2 processor2 = new CustomHeaderCellProcessor2();
            reportBuilder.AddColumn("#", i => i)
                .AddHeaderProcessors(processor1, processor2);

            IReportTable<ReportCell> _ = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            processor1.CallsCount.Should().Be(1);
            processor2.CallsCount.Should().Be(1);
        }

        private abstract class CustomHeaderCellProcessor : IReportCellProcessor<int>
        {
            public void Process(ReportCell cell, int data)
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
