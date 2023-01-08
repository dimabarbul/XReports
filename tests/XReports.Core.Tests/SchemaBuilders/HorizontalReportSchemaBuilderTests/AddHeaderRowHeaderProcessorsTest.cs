using System.Linq;
using FluentAssertions;
using XReports.Core.Tests.Extensions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddHeaderProcessorsTest"/>
    public class AddHeaderRowHeaderProcessorsTest
    {
        [Fact]
        public void AddHeaderRowHeaderProcessorsShouldAddProcessorsToBeCalledForHeaderCell()
        {
            HorizontalReportSchemaBuilder<int> reportBuilder = new HorizontalReportSchemaBuilder<int>();
            CustomHeaderCellProcessor1 processor1 = new CustomHeaderCellProcessor1();
            CustomHeaderCellProcessor2 processor2 = new CustomHeaderCellProcessor2();
            reportBuilder.AddRow("#", i => i);
            reportBuilder.AddHeaderRow("Header", i => i)
                .AddHeaderProcessors(processor1, processor2);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Enumerate();

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
