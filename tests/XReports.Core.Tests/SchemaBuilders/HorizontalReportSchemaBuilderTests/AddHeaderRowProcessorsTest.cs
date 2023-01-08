using System.Collections.Generic;
using FluentAssertions;
using XReports.Core.Tests.Extensions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddProcessorsTest"/>
    public class AddHeaderRowProcessorsTest
    {
        [Fact]
        public void AddHeaderRowProcessorsShouldAddProcessorsToBeCalledDuringForEachColumn()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor2 processor2 = new CustomProcessor2();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddHeaderRow("Header", s => s.ToUpperInvariant())
                .AddProcessors(processor1, processor2);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
                "Test2",
            });
            table.Enumerate();

            processor1.ProcessedData.Should().Equal("TEST", "TEST2");
            processor2.ProcessedData.Should().Equal("TEST", "TEST2");
        }

        [Fact]
        public void AddHeaderRowProcessorsShouldHaveAccessToAllProperties()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            PropertyCheckProcessor processor = new PropertyCheckProcessor();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddHeaderRow("Value", s => s.ToUpperInvariant())
                .AddProperties(new CustomProperty1())
                .AddProcessors(processor);
            // global property won't be available as it's not added to header cells
            reportBuilder.AddGlobalProperties(new CustomProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });
            table.Enumerate();

            processor.Properties.Should().ContainSameOrEqualElements(new ReportCellProperty[]
            {
                new CustomProperty1(),
            });
        }

        private abstract class CustomProcessor : IReportCellProcessor<string>
        {
            public List<string> ProcessedData { get; } = new List<string>();

            public void Process(ReportCell cell, string entity)
            {
                this.ProcessedData.Add(cell.GetValue<string>());
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
