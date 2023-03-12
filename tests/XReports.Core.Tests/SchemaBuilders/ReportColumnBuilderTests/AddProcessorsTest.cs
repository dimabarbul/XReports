using System;
using System.Collections.Generic;
using FluentAssertions;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellsProviders;
using XReports.Table;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportColumnBuilderTests
{
    public class AddProcessorsTest
    {
        [Fact]
        public void AddProcessorsShouldAddProcessorsToBeCalledDuringForEachRow()
        {
            ReportColumnBuilder<string> builder = new ReportColumnBuilder<string>(
                "Value", new ComputedValueReportCellsProvider<string, string>(x => x));
            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor2 processor2 = new CustomProcessor2();
            builder.AddProcessors(processor1, processor2);
            IReportColumn<string> provider = builder.Build(Array.Empty<ReportCellProperty>());

            provider.CreateCell("Test");
            provider.CreateCell("Test2");

            processor1.ProcessedData.Should().Equal("Test", "Test2");
            processor2.ProcessedData.Should().Equal("Test", "Test2");
        }

        [Fact]
        public void AddProcessorsShouldThrowWhenSomeProcessorIsNull()
        {
            ReportColumnBuilder<string> builder = new ReportColumnBuilder<string>(
                "Value", new ComputedValueReportCellsProvider<string, string>(x => x));

            Action action = () => builder.AddProcessors(new CustomProcessor1(), new CustomProcessor2(), null);

            action.Should().ThrowExactly<ArgumentException>();
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
