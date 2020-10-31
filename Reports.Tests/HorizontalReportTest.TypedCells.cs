using System;
using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.ValueFormatters;
using Reports.ValueProviders;
using Xunit;

namespace Reports.Tests
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_IntegerRow_CorrectDisplayValue()
        {
            HorizontalReportBuilder<int> reportBuilder = new HorizontalReportBuilder<int>();
            reportBuilder.AddRow("#", i => i);

            IReportTable table = reportBuilder.Build(new[]
            {
                3,
                6,
            });

            IReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().BeEmpty();

            IReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].DisplayValue.Should().Be("#");
            cells[0][0].ValueType.Should().Be(typeof(string));
            cells[0][1].DisplayValue.Should().Be("3");
            cells[0][1].ValueType.Should().Be(typeof(int));
            cells[0][2].DisplayValue.Should().Be("6");
            cells[0][2].ValueType.Should().Be(typeof(int));
        }

        [Fact]
        public void Build_DecimalRowWithoutRounding_CorrectDisplayValue()
        {
            HorizontalReportBuilder<decimal> reportBuilder = new HorizontalReportBuilder<decimal>();
            reportBuilder.AddRow("Score", d => d)
                .SetValueFormatter(new DecimalValueFormatter(2));

            IReportTable table = reportBuilder.Build(new[]
            {
                3m,
                6.5m,
            });

            IReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].DisplayValue.Should().Be("Score");
            cells[0][0].ValueType.Should().Be(typeof(string));
            cells[0][1].DisplayValue.Should().Be("3.00");
            cells[0][1].ValueType.Should().Be(typeof(decimal));
            cells[0][2].DisplayValue.Should().Be("6.50");
            cells[0][2].ValueType.Should().Be(typeof(decimal));
        }

        [Fact]
        public void Build_DecimalRowWithRounding_CorrectDisplayValue()
        {
            HorizontalReportBuilder<decimal> reportBuilder = new HorizontalReportBuilder<decimal>();
            reportBuilder.AddRow("Score", d => d)
                .SetValueFormatter(new DecimalValueFormatter(0));

            IReportTable table = reportBuilder.Build(new[]
            {
                3m,
                6.5m,
            });

            IReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].DisplayValue.Should().Be("Score");
            cells[0][0].ValueType.Should().Be(typeof(string));
            cells[0][1].DisplayValue.Should().Be("3");
            cells[0][1].ValueType.Should().Be(typeof(decimal));
            cells[0][2].DisplayValue.Should().Be("7");
            cells[0][2].ValueType.Should().Be(typeof(decimal));
        }

        [Fact]
        public void Build_DateTimeRowWithFormat_CorrectDisplayValue()
        {
            HorizontalReportBuilder<DateTime> reportBuilder = new HorizontalReportBuilder<DateTime>();
            reportBuilder.AddRow("The Date", d => d)
                .SetValueFormatter(new DateTimeValueFormatter("MM/dd/yyyy"));
            reportBuilder.AddRow("Next Day", new CallbackComputedValueProvider<DateTime, DateTime>(d => d.AddDays(1)))
                .SetValueFormatter(new DateTimeValueFormatter("MM/dd/yyyy"));

            IReportTable table = reportBuilder.Build(new[]
            {
                new DateTime(2020, 10, 24, 20, 25, 00),
            });

            IReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].DisplayValue.Should().Be("The Date");
            cells[0][0].ValueType.Should().Be(typeof(string));
            cells[0][1].DisplayValue.Should().Be("10/24/2020");
            cells[0][1].ValueType.Should().Be(typeof(DateTime));
            cells[1][0].DisplayValue.Should().Be("Next Day");
            cells[1][0].ValueType.Should().Be(typeof(string));
            cells[1][1].DisplayValue.Should().Be("10/25/2020");
            cells[1][1].ValueType.Should().Be(typeof(DateTime));
        }
    }
}
