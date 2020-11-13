using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Xunit;

namespace Reports.Tests.Builders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_WithCustomHeaderProperty_CorrectProperties()
        {
            HorizontalReportBuilder<string> reportBuilder = new HorizontalReportBuilder<string>();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddTitleProperty("Value", new CustomTitleProperty());

            IReportTable<ReportCell> table = reportBuilder.Build(new []
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
            // HorizontalReportBuilder<string> reportBuilder = new HorizontalReportBuilder<string>();
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
            // HorizontalReportBuilder<string> reportBuilder = new HorizontalReportBuilder<string>();
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
