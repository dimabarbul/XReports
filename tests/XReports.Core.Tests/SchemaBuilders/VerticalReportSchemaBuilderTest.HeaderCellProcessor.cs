using System.Linq;
using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders
{
    public partial class VerticalReportSchemaBuilderTest
    {
        [Fact]
        public void BuildShouldCallHeaderProcessor()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = new();
            CustomHeaderCellProcessor processor = new();
            reportBuilder.AddColumn("#", i => i)
                .AddHeaderProcessors(processor);

            IReportTable<ReportCell> _ = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());

            processor.CallsCount.Should().Be(1);
        }

        private class CustomHeaderCellProcessor : IReportCellProcessor<int>
        {
            public int CallsCount { get; private set; }

            public void Process(ReportCell cell, int data)
            {
                this.CallsCount++;
            }
        }
    }
}
