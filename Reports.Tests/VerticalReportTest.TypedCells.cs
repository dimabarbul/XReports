using System;
using FluentAssertions;
using Reports.Builders;
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

            table.Cells.Should().HaveCount(3);
            table.Cells[0][0].DisplayValue.Should().Be("#");
            table.Cells[0][0].ValueType.Should().Be(typeof(string));
            table.Cells[1][0].DisplayValue.Should().Be("3");
            table.Cells[1][0].ValueType.Should().Be(typeof(int));
            table.Cells[2][0].DisplayValue.Should().Be("6");
            table.Cells[2][0].ValueType.Should().Be(typeof(int));
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

            table.Cells.Should().HaveCount(3);
            table.Cells[0][0].DisplayValue.Should().Be("Score");
            table.Cells[0][0].ValueType.Should().Be(typeof(string));
            table.Cells[1][0].DisplayValue.Should().Be("3.00");
            table.Cells[1][0].ValueType.Should().Be(typeof(decimal));
            table.Cells[2][0].DisplayValue.Should().Be("6.50");
            table.Cells[2][0].ValueType.Should().Be(typeof(decimal));
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

            table.Cells.Should().HaveCount(3);
            table.Cells[0][0].DisplayValue.Should().Be("Score");
            table.Cells[0][0].ValueType.Should().Be(typeof(string));
            table.Cells[1][0].DisplayValue.Should().Be("3");
            table.Cells[1][0].ValueType.Should().Be(typeof(decimal));
            table.Cells[2][0].DisplayValue.Should().Be("7");
            table.Cells[2][0].ValueType.Should().Be(typeof(decimal));
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

            table.Cells.Should().HaveCount(2);
            table.Cells[0][0].DisplayValue.Should().Be("The Date");
            table.Cells[0][0].ValueType.Should().Be(typeof(string));
            table.Cells[0][1].DisplayValue.Should().Be("Next Day");
            table.Cells[0][1].ValueType.Should().Be(typeof(string));
            table.Cells[1][0].DisplayValue.Should().Be("10/24/2020");
            table.Cells[1][0].ValueType.Should().Be(typeof(DateTime));
            table.Cells[1][1].DisplayValue.Should().Be("10/25/2020");
            table.Cells[1][1].ValueType.Should().Be(typeof(DateTime));
        }
    }
}
