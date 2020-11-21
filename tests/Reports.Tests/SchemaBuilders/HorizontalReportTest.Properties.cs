using FluentAssertions;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Reports.SchemaBuilders;
using Xunit;

namespace Reports.Tests.SchemaBuilders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_WithCustomProperty_CorrectProperties()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s)
                .AddProcessors(new CustomPropertyProcessor());

            var schema = reportBuilder.BuildSchema();
            IReportTable<ReportCell> table = schema.BuildReportTable(new []
            {
                "Test",
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].Properties.Should().BeEmpty();
            cells[0][1].Properties.Should()
                .HaveCount(1).And
                .ContainSingle(p => p is CustomProperty && ((CustomProperty) p).Assigned);
        }

        [Fact]
        public void Build_CustomPropertyWithoutProcessor_CorrectProperties()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s)
                .AddProperties(new CustomProperty());

            var schema = reportBuilder.BuildSchema();
            IReportTable<ReportCell> table = schema.BuildReportTable(new []
            {
                "Test",
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].Properties.Should().BeEmpty();
            cells[0][1].Properties.Should()
                .HaveCount(1).And
                .AllBeOfType<CustomProperty>();
        }

        private class CustomPropertyProcessor : IReportCellProcessor<string>
        {
            public void Process(ReportCell cell, string data)
            {
                cell.AddProperty(new CustomProperty(true));
            }
        }

        private class CustomProperty : ReportCellProperty
        {
            public bool Assigned { get; }

            public CustomProperty(bool assigned = false)
            {
                this.Assigned = assigned;
            }
        }
    }
}
