using System;
using System.Globalization;
using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void BuildShouldSupportColumnsWithDifferentTypes()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = new VerticalReportSchemaBuilder<int>();
            reportBuilder.AddColumn("#", i => i);
            reportBuilder.AddColumn("Today", new CallbackValueProvider<DateTime>(() => DateTime.Today));
            reportBuilder.AddColumn("ToString()", i => i.ToString(CultureInfo.CurrentCulture));

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                3,
                6,
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].ValueType.Should().Be(typeof(string));
            headerCells[0][0].GetValue<string>().Should().Be("#");
            headerCells[0][1].ValueType.Should().Be(typeof(string));
            headerCells[0][1].GetValue<string>().Should().Be("Today");
            headerCells[0][2].ValueType.Should().Be(typeof(string));
            headerCells[0][2].GetValue<string>().Should().Be("ToString()");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].ValueType.Should().Be(typeof(int));
            cells[0][0].GetValue<int>().Should().Be(3);
            cells[0][1].ValueType.Should().Be(typeof(DateTime));
            cells[0][1].GetValue<DateTime>().Should().Be(DateTime.Today);
            cells[0][2].ValueType.Should().Be(typeof(string));
            cells[0][2].GetValue<string>().Should().Be("3");
            cells[1][0].ValueType.Should().Be(typeof(int));
            cells[1][0].GetValue<int>().Should().Be(6);
            cells[1][1].ValueType.Should().Be(typeof(DateTime));
            cells[1][1].GetValue<DateTime>().Should().Be(DateTime.Today);
            cells[1][2].ValueType.Should().Be(typeof(string));
            cells[1][2].GetValue<string>().Should().Be("6");
        }

        private class CallbackValueProvider<TValue> : IValueProvider<TValue>
        {
            private readonly Func<TValue> callback;

            public CallbackValueProvider(Func<TValue> callback)
            {
                this.callback = callback;
            }

            public TValue GetValue()
            {
                return this.callback();
            }
        }
    }
}
