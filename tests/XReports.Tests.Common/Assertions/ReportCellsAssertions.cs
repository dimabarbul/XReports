using System.Collections.Generic;
using System.Linq;
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
    }
}
