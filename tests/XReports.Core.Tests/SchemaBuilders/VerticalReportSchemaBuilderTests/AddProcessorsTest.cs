using System.Collections.Generic;
using FluentAssertions;
using XReports.Core.Tests.Extensions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddProcessorsTest"/>
    public class AddProcessorsTest
    {
        [Fact]
        public void AddProcessorsShouldAddProcessorsToBeCalledDuringForEachRow()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor2 processor2 = new CustomProcessor2();
            reportBuilder.AddColumn("Value", s => s)
                .AddProcessors(processor1, processor2);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            PropertyCheckProcessor processor = new PropertyCheckProcessor();
            reportBuilder.AddColumn("Value", s => s)
                .AddProperties(new CustomProperty1())
                .AddProcessors(processor);
            reportBuilder.AddGlobalProperties(new CustomProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });
            table.Enumerate();

            processor.Properties.Should().BeEquivalentTo(new ReportCellProperty[]
            {
                new CustomProperty1(),
                new CustomProperty2(),
            });
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

        private class CustomProperty1 : ReportCellProperty
        {
        }

        private class CustomProperty2 : ReportCellProperty
        {
        }

        private class PropertyCheckProcessor : IReportCellProcessor<string>
        {
            public IReadOnlyList<ReportCellProperty> Properties { get; set; }

            public void Process(ReportCell cell, string entity)
            {
                this.Properties = new List<ReportCellProperty>(cell.Properties);
            }
        }
    }
}
