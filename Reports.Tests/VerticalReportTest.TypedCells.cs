using System;
using FluentAssertions;
using Reports.Builders;
using Reports.Interfaces;
using Reports.Models;
using Reports.ValueFormatters;
using Reports.ValueProviders;
using Xunit;

namespace Reports.Tests
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_IntegerColumn_CorrectDisplayValue()
        {
            VerticalReportBuilder<int> reportBuilder = new VerticalReportBuilder<int>();
            reportBuilder.AddColumn("#", i => i);

            ReportTable table = reportBuilder.Build(new[]
            {
                3,
                6,
            });

            IReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderCells);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].DisplayValue.Should().Be("#");
            headerCells[0][0].ValueType.Should().Be(typeof(string));

            IReportCell[][] cells = this.GetCellsAsArray(table.Cells);
            cells.Should().HaveCount(2);
            cells[0][0].DisplayValue.Should().Be("3");
            cells[0][0].ValueType.Should().Be(typeof(int));
            cells[1][0].DisplayValue.Should().Be("6");
            cells[1][0].ValueType.Should().Be(typeof(int));
        }

        [Fact]
        public void Build_DecimalColumnWithoutRounding_CorrectDisplayValue()
        {
            VerticalReportBuilder<decimal> reportBuilder = new VerticalReportBuilder<decimal>();
            reportBuilder.AddColumn("Score", d => d);
            reportBuilder.SetColumnValueFormatter("Score", new DecimalValueFormatter(2));

            ReportTable table = reportBuilder.Build(new[]
            {
                3m,
                6.5m,
            });

            IReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderCells);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].DisplayValue.Should().Be("Score");
            headerCells[0][0].ValueType.Should().Be(typeof(string));

            IReportCell[][] cells = this.GetCellsAsArray(table.Cells);
            cells.Should().HaveCount(2);
            cells[0][0].DisplayValue.Should().Be("3.00");
            cells[0][0].ValueType.Should().Be(typeof(decimal));
            cells[1][0].DisplayValue.Should().Be("6.50");
            cells[1][0].ValueType.Should().Be(typeof(decimal));
        }

        [Fact]
        public void Build_DecimalColumnWithRounding_CorrectDisplayValue()
        {
            VerticalReportBuilder<decimal> reportBuilder = new VerticalReportBuilder<decimal>();
            reportBuilder.AddColumn("Score", d => d);
            reportBuilder.SetColumnValueFormatter("Score", new DecimalValueFormatter(0));

            ReportTable table = reportBuilder.Build(new[]
            {
                3m,
                6.5m,
            });

            IReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderCells);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].DisplayValue.Should().Be("Score");
            headerCells[0][0].ValueType.Should().Be(typeof(string));

            IReportCell[][] cells = this.GetCellsAsArray(table.Cells);
            cells.Should().HaveCount(2);
            cells[0][0].DisplayValue.Should().Be("3");
            cells[0][0].ValueType.Should().Be(typeof(decimal));
            cells[1][0].DisplayValue.Should().Be("7");
            cells[1][0].ValueType.Should().Be(typeof(decimal));
        }

        [Fact]
        public void Build_DateTimeColumnWithFormat_CorrectDisplayValue()
        {
            VerticalReportBuilder<DateTime> reportBuilder = new VerticalReportBuilder<DateTime>();
            reportBuilder.AddColumn("The Date", d => d);
            reportBuilder.SetColumnValueFormatter("The Date", new DateTimeValueFormatter("MM/dd/yyyy"));
            reportBuilder.AddColumn("Next Day", new CallbackComputedValueProvider<DateTime, DateTime>(d => d.AddDays(1)));
            reportBuilder.SetColumnValueFormatter("Next Day", new DateTimeValueFormatter("MM/dd/yyyy"));

            ReportTable table = reportBuilder.Build(new[]
            {
                new DateTime(2020, 10, 24, 20, 25, 00),
            });

            IReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderCells);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].DisplayValue.Should().Be("The Date");
            headerCells[0][0].ValueType.Should().Be(typeof(string));
            headerCells[0][1].DisplayValue.Should().Be("Next Day");
            headerCells[0][1].ValueType.Should().Be(typeof(string));

            IReportCell[][] cells = this.GetCellsAsArray(table.Cells);
            cells.Should().HaveCount(1);
            cells[0][0].DisplayValue.Should().Be("10/24/2020");
            cells[0][0].ValueType.Should().Be(typeof(DateTime));
            cells[0][1].DisplayValue.Should().Be("10/25/2020");
            cells[0][1].ValueType.Should().Be(typeof(DateTime));
        }
    }
}
