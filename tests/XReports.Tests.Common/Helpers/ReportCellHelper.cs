using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using XReports.Table;
using XReports.Tests.Common.Assertions;

namespace XReports.Tests.Common.Helpers
{
    public static class ReportCellHelper
    {
        public static ReportCell CreateReportCell<TValue>(
            TValue value,
            int rowSpan = 1,
            int columnSpan = 1,
            params IReportCellProperty[] properties)
        {
            ReportCell cell = new ReportCell();
            cell.SetValue(value);
            cell.RowSpan = rowSpan;
            cell.ColumnSpan = columnSpan;

            cell.AddProperties(properties);

            return cell;
        }

        public static ReportCell CreateReportCell<TValue>(
            TValue value,
            params IReportCellProperty[] properties)
        {
            return CreateReportCell(value, 1, 1, properties);
        }

        public static Action<ReportCell> GetCellInspector(ReportCell expectedCell)
        {
            if (expectedCell == null)
            {
                return actual =>
                {
                    actual.Should().BeNull();
                };
            }

            return actual =>
            {
                actual.GetUnderlyingValue().Should().Be(expectedCell.GetUnderlyingValue());
                actual.ValueType.Should().Be(expectedCell.ValueType);
                actual.ColumnSpan.Should().Be(expectedCell.ColumnSpan);
                actual.RowSpan.Should().Be(expectedCell.RowSpan);
                actual.Properties.Should().BeEquivalentTo(expectedCell.Properties);
            };
        }

        public static bool AreObjectCollectionsShallowlyEquivalent(
            IEnumerable<object> actual,
            IEnumerable<object> expected)
        {
            List<object> properties = new List<object>(expected);

            foreach (object property in actual)
            {
                int index = properties.FindIndex(e => AreObjectsShallowlyEqual(property, e));
                if (index == -1)
                {
                    return false;
                }

                properties.RemoveAt(index);
            }

            return properties.Count == 0;
        }

        private static bool AreObjectsShallowlyEqual(object actual, object expected)
        {
            if (actual.GetType() != expected.GetType())
            {
                return false;
            }

            foreach (PropertyInfo propertyInfo in actual.GetType().GetProperties())
            {
                object actualValue = propertyInfo.GetValue(actual);
                object expectedValue = propertyInfo.GetValue(expected);
                if (!AreValuesEqual(actualValue, expectedValue))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool AreValuesEqual(object actualValue, object expectedValue)
        {
            return (actualValue == null && expectedValue == null)
                || (actualValue != null && actualValue.Equals(expectedValue));
        }
    }
}
