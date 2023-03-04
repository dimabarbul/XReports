using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Collections;
using XReports.Models;

namespace XReports.Tests.Common.Assertions
{
    public class ReportCellsAssertions : GenericCollectionAssertions<IEnumerable<ReportCell>>
    {
        public ReportCellsAssertions(IEnumerable<IEnumerable<ReportCell>> actualValue)
            : base(actualValue)
        {
        }

        protected override string Identifier => "report cells";

        public AndConstraint<ReportCellsAssertions> BeEquivalentTo(IEnumerable<IEnumerable<ReportCell>> expected)
        {
            ReportCell[][] actualCells = this.Subject.Clone();
            ReportCell[][] expectedCells = expected.Clone();

            actualCells.Should().HaveSameCount(expectedCells, "report should have correct count of rows");

            for (int i = 0; i < actualCells.Length; i++)
            {
                actualCells[i].Should().SatisfyRespectively(
                    expectedCells[i].Select(this.GetInspector).ToArray(),
                    $"row at index {i} should contain correct cells");
            }

            return new AndConstraint<ReportCellsAssertions>(this);
        }

        public AndConstraint<ReportCellsAssertions> BeEquivalentTo(IEnumerable<IEnumerable<object>> expected)
        {
            ReportCell[][] actualCells = this.Subject.Clone();
            ReportCellData[][] expectedCells = this.ConvertExpectedCells(expected);

            actualCells.Should().HaveSameCount(expectedCells, "report should have correct count of rows");

            for (int i = 0; i < actualCells.Length; i++)
            {
                actualCells[i].Should().HaveSameCount(expectedCells[i], $"report row {i + 1} should have correct count of cells");

                for (int j = 0; j < actualCells[i].Length; j++)
                {
                    actualCells[i][j]
                        .Should($"report cell in row {i + 1} in column {j + 1}")
                        .Be(expectedCells[i][j]);
                }
            }

            return new AndConstraint<ReportCellsAssertions>(this);
        }

        private Action<ReportCell> GetInspector(ReportCell expectedCell)
        {
            return actual =>
            {
                actual.GetUnderlyingValue().Should().Be(expectedCell.GetUnderlyingValue());
                actual.ValueType.Should().Be(expectedCell.ValueType);
                actual.ColumnSpan.Should().Be(expectedCell.ColumnSpan);
                actual.RowSpan.Should().Be(expectedCell.RowSpan);
                actual.Properties.Should().Equal(expectedCell.Properties, this.ArePropertiesEqual);
            };
        }

        private ReportCellData[][] ConvertExpectedCells(IEnumerable<IEnumerable<object>> expected)
        {
            return expected
                .Select(row => row
                    .Select(value => value is null ?
                        null :
                        (value as ReportCellData ?? new ReportCellData(value)))
                    .ToArray())
                .ToArray();
        }

        private bool ArePropertiesEqual(ReportCellProperty actual, ReportCellProperty expected)
        {
            if (actual.GetType() != expected.GetType())
            {
                return false;
            }

            foreach (PropertyInfo propertyInfo in actual.GetType().GetProperties())
            {
                object actualValue = propertyInfo.GetValue(actual);
                object expectedValue = propertyInfo.GetValue(expected);
                if (!this.AreObjectsEqual(actualValue, expectedValue))
                {
                    return false;
                }
            }

            return true;
        }

        private bool AreObjectsEqual(object actualValue, object expectedValue)
        {
            return (actualValue == null && expectedValue == null)
                || (actualValue != null && actualValue.Equals(expectedValue));
        }
    }
}
