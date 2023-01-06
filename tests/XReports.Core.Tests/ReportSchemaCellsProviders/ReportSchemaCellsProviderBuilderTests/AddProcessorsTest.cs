using System;
using System.Collections.Generic;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.ReportSchemaCellsProviders;
using Xunit;

namespace XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests
{
    public class AddProcessorsTest
    {
        [Fact]
        public void AddProcessorsShouldAddProcessorsToBeCalledDuringForEachRow()
        {
            ReportSchemaCellsProviderBuilder<string> builder = new ReportSchemaCellsProviderBuilder<string>(
                "Value", new ComputedValueReportCellsProvider<string, string>(x => x));
            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor2 processor2 = new CustomProcessor2();
            builder.AddProcessors(processor1, processor2);
            ReportSchemaCellsProvider<string> provider = builder.Build(ArraySegment<ReportCellProperty>.Empty);

            provider.CreateCell("Test");
            provider.CreateCell("Test2");

            processor1.ProcessedData.Should().Equal("Test", "Test2");
            processor2.ProcessedData.Should().Equal("Test", "Test2");
        }

        private abstract class CustomProcessor : IReportCellProcessor<string>
        {
            public List<string> ProcessedData { get; } = new List<string>();

            public void Process(ReportCell cell, string entity)
            {
                this.ProcessedData.Add(entity);
            }
        }

        private class CustomProcessor1 : CustomProcessor
        {
        }

        private class CustomProcessor2 : CustomProcessor
        {
        }
    }
}
