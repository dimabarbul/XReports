using System;
using System.Globalization;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void BuildShouldSupportColumnsWithDifferentTypes()
        {
            DateTime today = DateTime.Today;
            VerticalReportSchemaBuilder<int> reportBuilder = new();
            reportBuilder.AddColumn("#", i => i);
            reportBuilder.AddColumn("Today", new CallbackValueProvider<DateTime>(() => today));
            reportBuilder.AddColumn("ToString()", i => i.ToString(CultureInfo.CurrentCulture));

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                3,
                6,
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { "#", "Today", "ToString()" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { 3, today, "3" },
                new object[] { 6, today, "6" },
            });
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
