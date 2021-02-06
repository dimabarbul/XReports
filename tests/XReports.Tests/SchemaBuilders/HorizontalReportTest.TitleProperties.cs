using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_WithCustomHeaderProperty_CorrectProperties()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s)
                .AddHeaderProperties(new CustomTitleProperty());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].Properties.Should()
                .HaveCount(1).And
                .ContainItemsAssignableTo<CustomTitleProperty>();
        }

        [Fact(Skip = "Complex title is not implemented yet")]
        public void Build_WithCustomPropertyForComplexHeaderUsingAddHeaderProperty_CorrectProperties()
        {
            // HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            // reportBuilder.AddRow("Value", s => s);
            // reportBuilder.AddComplexTitle(0, "Complex", "Value");
            // reportBuilder.AddTitleProperty("Complex", new CustomTitleProperty());
            //
            // IReportTable<ReportCell> table = reportBuilder.Build(new string[] { });
            //
            // ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            // cells.Should().HaveCount(2);
            // cells[0][0].Properties.Should()
            //     .HaveCount(1).And
            //     .ContainItemsAssignableTo<CustomTitleProperty>();
        }

        [Fact(Skip = "Complex title is not implemented yet")]
        public void Build_WithCustomPropertyForComplexHeaderUsingProcessor_CorrectProperties()
        {
            // HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            // reportBuilder.AddRow("Value", s => s);
            // reportBuilder.AddComplexTitle(0, "Complex", "Value");
            // reportBuilder.AddTitleCellProcessor(new CustomTitlePropertyProcessor());
            //
            // IReportTable<ReportCell> table = reportBuilder.Build(new string[] { });
            //
            // ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            // cells.Should().HaveCount(2);
            // cells[0][0].Properties.Should()
            //     .HaveCount(1).And
            //     .ContainItemsAssignableTo<CustomTitleProperty>();
        }

        private class CustomTitleProperty : ReportCellProperty
        {
        }

        // private class CustomTitlePropertyProcessor : IReportCellProcessor
        // {
        //     public void Process(ReportCell cell)
        //     {
        //         if (cell.InternalValue == "Complex")
        //         {
        //             cell.AddProperty(new CustomTitleProperty());
        //         }
        //     }
        // }
    }
}
