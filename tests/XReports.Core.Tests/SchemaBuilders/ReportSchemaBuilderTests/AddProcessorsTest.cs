using System.Collections.Generic;
using FluentAssertions;
using XReports.Core.Tests.Extensions;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    /// <seealso cref="ReportColumnBuilderTests.AddProcessorsTest"/>
    public class AddProcessorsTest
    {
        [Fact]
        public void AddProcessorsShouldAddProcessorsToBeCalledDuringForEachRow()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor2 processor2 = new CustomProcessor2();
            reportBuilder.AddColumn("Value", s => s)
                .AddProcessors(processor1, processor2);

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(new[]
            {
                "Test",
                "Test2",
            });
            table.Enumerate();

            processor1.ProcessedData.Should().Equal("Test", "Test2");
            processor2.ProcessedData.Should().Equal("Test", "Test2");
        }

        [Fact]
        public void AddProcessorsShouldAddProcessorsToBeCalledDuringForEachRowForHorizontal()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor2 processor2 = new CustomProcessor2();
            reportBuilder.AddColumn("Value", s => s)
                .AddProcessors(processor1, processor2);

            IReportTable<ReportCell> table = reportBuilder.BuildHorizontalSchema(0).BuildReportTable(new[]
            {
                "Test",
                "Test2",
            });
            table.Enumerate();

            processor1.ProcessedData.Should().Equal("Test", "Test2");
            processor2.ProcessedData.Should().Equal("Test", "Test2");
        }

        [Fact]
        public void AddProcessorsShouldHaveAccessToAllProperties()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            PropertyCheckProcessor processor = new PropertyCheckProcessor();
            reportBuilder.AddColumn("Value", s => s)
                .AddProperties(new CustomProperty1())
                .AddProcessors(processor);
            reportBuilder.AddGlobalProperties(new CustomProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(new[]
            {
                "Test",
            });
            table.Enumerate();

            processor.Properties.Should().BeEquivalentTo(new IReportCellProperty[]
            {
                new CustomProperty1(),
                new CustomProperty2(),
            });
        }

        private abstract class CustomProcessor : IReportCellProcessor<string>
        {
            public List<string> ProcessedData { get; } = new List<string>();

            public void Process(ReportCell cell, string item)
            {
                this.ProcessedData.Add(item);
            }
        }

        private class CustomProcessor1 : CustomProcessor
        {
        }

        private class CustomProcessor2 : CustomProcessor
        {
        }

        private class CustomProperty1 : IReportCellProperty
        {
        }

        private class CustomProperty2 : IReportCellProperty
        {
        }

        private class PropertyCheckProcessor : IReportCellProcessor<string>
        {
            public IReadOnlyList<IReportCellProperty> Properties { get; set; }

            public void Process(ReportCell cell, string item)
            {
                this.Properties = new List<IReportCellProperty>(cell.Properties);
            }
        }
    }
}
