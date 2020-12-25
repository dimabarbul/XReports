namespace XReports.Tests.SchemaBuilders
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
        //     IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
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
