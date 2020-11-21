using System.Linq;
using FluentAssertions;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Reports.SchemaBuilders;
using Xunit;

namespace Reports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        // [Fact]
        // public void Build_WithHeaderCellProcessor_HeaderProcessed()
        // {
        //     VerticalReportSchemaBuilder<int> reportBuilder = new VerticalReportSchemaBuilder<int>();
        //     reportBuilder.AddColumn("#", i => i);
        //     reportBuilder.AddHeaderCellProcessor(new CustomHeaderCellProcessor());
        //
        //     var schema = reportBuilder.BuildSchema();
        //     IReportTable<ReportCell> table = schema.BuildReportTable(Enumerable.Empty<int>());
        //
        //     ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
        //     headerCells.Should().HaveCount(1);
        //     headerCells[0][0].GetValue<string>().Should().Be("-- # --");
        // }
        //
        // public class CustomHeaderCellProcessor : IReportCellProcessor<int>
        // {
        //     public void Process(ReportCell cell, int data)
        //     {
        //         cell.InternalValue = $"-- {cell.InternalValue} --";
        //     }
        // }
    }
}
