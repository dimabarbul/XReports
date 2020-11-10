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
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_IntegerColumn_CorrectDisplayValue()
        {
            VerticalReportBuilder<int> reportBuilder = new VerticalReportBuilder<int>();
            reportBuilder.AddColumn("#", i => i);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                3,
                6,
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].ValueType.Should().Be(typeof(string));
            headerCells[0][0].GetValue<string>().Should().Be("#");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].ValueType.Should().Be(typeof(int));
            cells[0][0].GetValue<int>().Should().Be(3);
            cells[1][0].ValueType.Should().Be(typeof(int));
            cells[1][0].GetValue<int>().Should().Be(6);
        }

        [Fact(Skip = "Formatting is to be moved to converting")]
        public void Build_DecimalColumnWithoutRounding_CorrectDisplayValue()
        {
            VerticalReportBuilder<decimal> reportBuilder = new VerticalReportBuilder<decimal>();
            reportBuilder.AddColumn("Score", d => d);
                // .SetValueFormatter(new DecimalValueFormatter(2));

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                3m,
                6.5m,
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].InternalValue.Should().Be("Score");
            headerCells[0][0].ValueType.Should().Be(typeof(string));

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].InternalValue.Should().Be("3.00");
            cells[0][0].ValueType.Should().Be(typeof(decimal));
            cells[1][0].InternalValue.Should().Be("6.50");
            cells[1][0].ValueType.Should().Be(typeof(decimal));
        }

        [Fact(Skip = "Formatting is to be moved to converting")]
        public void Build_DecimalColumnWithRounding_CorrectDisplayValue()
        {
            VerticalReportBuilder<decimal> reportBuilder = new VerticalReportBuilder<decimal>();
            reportBuilder.AddColumn("Score", d => d);
                // .SetValueFormatter(new DecimalValueFormatter(0));

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                3m,
                6.5m,
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].InternalValue.Should().Be("Score");
            headerCells[0][0].ValueType.Should().Be(typeof(string));

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].InternalValue.Should().Be("3");
            cells[0][0].ValueType.Should().Be(typeof(decimal));
            cells[1][0].InternalValue.Should().Be("7");
            cells[1][0].ValueType.Should().Be(typeof(decimal));
        }

        [Fact(Skip = "Formatting is to be moved to converting")]
        public void Build_DateTimeColumnWithFormat_CorrectDisplayValue()
        {
            VerticalReportBuilder<DateTime> reportBuilder = new VerticalReportBuilder<DateTime>();
            reportBuilder.AddColumn("The Date", d => d);
                // .SetValueFormatter(new DateTimeValueFormatter("MM/dd/yyyy"));
            reportBuilder.AddColumn("Next Day", new CallbackComputedValueProvider<DateTime, DateTime>(d => d.AddDays(1)));
                // .SetValueFormatter(new DateTimeValueFormatter("MM/dd/yyyy"));

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                new DateTime(2020, 10, 24, 20, 25, 00),
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].InternalValue.Should().Be("The Date");
            headerCells[0][0].ValueType.Should().Be(typeof(string));
            headerCells[0][1].InternalValue.Should().Be("Next Day");
            headerCells[0][1].ValueType.Should().Be(typeof(string));

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].InternalValue.Should().Be("10/24/2020");
            cells[0][0].ValueType.Should().Be(typeof(DateTime));
            cells[0][1].InternalValue.Should().Be("10/25/2020");
            cells[0][1].ValueType.Should().Be(typeof(DateTime));
        }
    }
}
