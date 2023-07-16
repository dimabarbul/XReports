using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using XReports.Core.Tests.Extensions;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.Table;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    /// <seealso cref="ReportColumnBuilderTests.AddGlobalProcessorsTest"/>
    public class AddGlobalProcessorsTest
    {
        [Fact]
        public void AddGlobalProcessorsShouldAddProcessorsToAllColumns()
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();
            schemaBuilder.AddColumn("Value", new ComputedValueReportCellProvider<int, int>(x => x));
            schemaBuilder.AddColumn("As string", new ComputedValueReportCellProvider<int, string>(x => x.ToString(CultureInfo.InvariantCulture)));

            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor2 processor2 = new CustomProcessor2();
            schemaBuilder.AddGlobalProcessors(processor1, processor2);

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(new[]
            {
                1,
                2,
            });
            table.Enumerate();

            processor1.ProcessedCells.Should().BeEquivalentTo(1, "1", 2, "2");
            processor2.ProcessedCells.Should().BeEquivalentTo(1, "1", 2, "2");
        }

        [Fact]
        public void AddGlobalProcessorsShouldAddProcessorsToAllColumnsAndRowsForHorizontal()
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();
            schemaBuilder.AddColumn("Value", new ComputedValueReportCellProvider<int, int>(x => x));
            schemaBuilder.AddColumn("As string", new ComputedValueReportCellProvider<int, string>(x => x.ToString(CultureInfo.InvariantCulture)));

            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor2 processor2 = new CustomProcessor2();
            schemaBuilder.AddGlobalProcessors(processor1, processor2);

            IReportTable<ReportCell> table = schemaBuilder.BuildHorizontalSchema(0).BuildReportTable(new[]
            {
                1,
                2,
            });
            table.Enumerate();

            processor1.ProcessedCells.Should().BeEquivalentTo(1, "1", 2, "2");
            processor2.ProcessedCells.Should().BeEquivalentTo(1, "1", 2, "2");
        }

        private class BaseCustomProcessor : IReportCellProcessor<int>
        {
            public List<object> ProcessedCells { get; } = new List<object>();

            public void Process(ReportCell cell, int item)
            {
                this.ProcessedCells.Add(cell.GetValue<object>());
            }
        }

        private class CustomProcessor1 : BaseCustomProcessor
        {
        }

        private class CustomProcessor2 : BaseCustomProcessor
        {
        }
    }
}
