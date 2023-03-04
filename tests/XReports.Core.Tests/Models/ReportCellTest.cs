using System;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using XReports.Models;
using Xunit;

namespace XReports.Core.Tests.Models
{
    public class ReportCellTest
    {
        [Fact]
        public void GetValueShouldConvertValue()
        {
            ReportCell reportCell = new ReportCell();
            reportCell.SetValue(1);

            reportCell.GetValue<decimal>().Should().Be(1m);
            reportCell.GetValue<string>().Should().Be("1");
        }

        [Fact]
        public void GetValueShouldConvertValueUsingCurrentCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            ReportCell reportCell = new ReportCell();
            reportCell.SetValue("1,23");

            reportCell.GetValue<decimal>().Should().Be(1.23m);
        }

        [Fact]
        public void GetValueShouldThrowWhenValueIsNull()
        {
            ReportCell reportCell = new ReportCell();
            reportCell.SetValue<string>(null);

            Action action = () => reportCell.GetValue<int>();
            action.Should().ThrowExactly<InvalidCastException>();
        }

        [Fact]
        public void GetValueShouldThrowWhenValueIsNotConvertible()
        {
            ReportCell reportCell = new ReportCell();
            reportCell.SetValue(1);

            Action action = () => reportCell.GetValue<DateTime>();

            action.Should().Throw<Exception>();
        }


        [Fact]
        public void GetNullableValueShouldReturnNullWhenValueIsNull()
        {
            ReportCell reportCell = new ReportCell();
            reportCell.SetValue<string>(null);

            reportCell.GetNullableValue<decimal>().Should().Be(null);
            reportCell.GetNullableValue<DateTime>().Should().Be(null);
        }

        [Fact]
        public void GetNullableValueShouldThrowWhenValueIsNotConvertible()
        {
            ReportCell reportCell = new ReportCell();
            reportCell.SetValue("1");

            Action action = () => reportCell.GetNullableValue<DateTime>();

            action.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData(typeof(int), 1)]
        [InlineData(typeof(string), "1")]
        public void GetUnderlyingValueShouldReturnOriginalValue(Type valueType, dynamic value)
        {
            ReportCell reportCell = new ReportCell();
            reportCell.SetValue(value);

            object underlyingValue = reportCell.GetUnderlyingValue();

            underlyingValue.Should().BeOfType(valueType);
            underlyingValue.Should().Be(value);
        }
    }
}
