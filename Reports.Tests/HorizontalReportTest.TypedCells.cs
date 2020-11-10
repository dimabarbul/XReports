using System;
using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Reports.ValueProviders;
using Xunit;

namespace Reports.Tests
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_IntegerRow_CorrectValue()
        {
            HorizontalReportBuilder<int> reportBuilder = new HorizontalReportBuilder<int>();
            reportBuilder.AddRow("#", i => i);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                3,
                6,
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().BeEmpty();

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].ValueType.Should().Be(typeof(string));
            cells[0][0].GetValue<string>().Should().Be("#");
            cells[0][1].ValueType.Should().Be(typeof(int));
            cells[0][1].GetValue<int>().Should().Be(3);
            cells[0][2].ValueType.Should().Be(typeof(int));
            cells[0][2].GetValue<int>().Should().Be(6);
        }

        [Fact(Skip = "Formatting is to be moved to converting")]
        public void Build_DecimalRowWithoutRounding_CorrectDisplayValue()
        {
            HorizontalReportBuilder<decimal> reportBuilder = new HorizontalReportBuilder<decimal>();
            reportBuilder.AddRow("Score", d => d);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                3m,
                6.5m,
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].GetValue<string>().Should().Be("Score");
            cells[0][0].ValueType.Should().Be(typeof(string));
            cells[0][1].GetValue<decimal>().Should().Be(3m);
            cells[0][1].ValueType.Should().Be(typeof(decimal));
            cells[0][2].GetValue<decimal>().Should().Be(6.5m);
            cells[0][2].ValueType.Should().Be(typeof(decimal));
        }

        [Fact(Skip = "Formatting is to be moved to converting")]
        public void Build_DecimalRowWithRounding_CorrectDisplayValue()
        {
            HorizontalReportBuilder<decimal> reportBuilder = new HorizontalReportBuilder<decimal>();
            reportBuilder.AddRow("Score", d => d);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                3m,
                6.5m,
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].ValueType.Should().Be(typeof(string));
            cells[0][0].GetValue<string>().Should().Be("Score");
            cells[0][1].ValueType.Should().Be(typeof(decimal));
            cells[0][1].GetValue<decimal>().Should().Be(3m);
            cells[0][2].ValueType.Should().Be(typeof(decimal));
            cells[0][2].GetValue<decimal>().Should().Be(7m);
        }

        [Fact(Skip = "Formatting is to be moved to converting")]
        public void Build_DateTimeRowWithFormat_CorrectDisplayValue()
        {
            HorizontalReportBuilder<DateTime> reportBuilder = new HorizontalReportBuilder<DateTime>();
            reportBuilder.AddRow("The Date", d => d);
                // .SetValueFormatter(new DateTimeValueFormatter("MM/dd/yyyy"));
            reportBuilder.AddRow("Next Day", new CallbackComputedValueProvider<DateTime, DateTime>(d => d.AddDays(1)));
                // .SetValueFormatter(new DateTimeValueFormatter("MM/dd/yyyy"));

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                new DateTime(2020, 10, 24, 20, 25, 00),
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].InternalValue.Should().Be("The Date");
            cells[0][0].ValueType.Should().Be(typeof(string));
            cells[0][1].InternalValue.Should().Be("10/24/2020");
            cells[0][1].ValueType.Should().Be(typeof(DateTime));
            cells[1][0].InternalValue.Should().Be("Next Day");
            cells[1][0].ValueType.Should().Be(typeof(string));
            cells[1][1].InternalValue.Should().Be("10/25/2020");
            cells[1][1].ValueType.Should().Be(typeof(DateTime));
        }
    }
}
