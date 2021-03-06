using System;
using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.ValueProviders;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_IntegerRow_CorrectValue()
        {
            HorizontalReportSchemaBuilder<int> reportBuilder = new HorizontalReportSchemaBuilder<int>();
            reportBuilder.AddRow("#", i => i);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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
            HorizontalReportSchemaBuilder<decimal> reportBuilder = new HorizontalReportSchemaBuilder<decimal>();
            reportBuilder.AddRow("Score", d => d);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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
            HorizontalReportSchemaBuilder<decimal> reportBuilder = new HorizontalReportSchemaBuilder<decimal>();
            reportBuilder.AddRow("Score", d => d);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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
            HorizontalReportSchemaBuilder<DateTime> reportBuilder = new HorizontalReportSchemaBuilder<DateTime>();
            reportBuilder.AddRow("The Date", d => d); // .SetValueFormatter(new DateTimeValueFormatter("MM/dd/yyyy"));
            reportBuilder.AddRow("Next Day", d => d.AddDays(1)); // .SetValueFormatter(new DateTimeValueFormatter("MM/dd/yyyy"));

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                new DateTime(2020, 10, 24, 20, 25, 00),
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].Value.Should().Be("The Date");
            cells[0][0].ValueType.Should().Be(typeof(string));
            cells[0][1].Value.Should().Be("10/24/2020");
            cells[0][1].ValueType.Should().Be(typeof(DateTime));
            cells[1][0].Value.Should().Be("Next Day");
            cells[1][0].ValueType.Should().Be(typeof(string));
            cells[1][1].Value.Should().Be("10/25/2020");
            cells[1][1].ValueType.Should().Be(typeof(DateTime));
        }
    }
}
