using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using XReports.Models;
using XReports.Tests.Common.Helpers;

namespace XReports.Tests.Common.Assertions
{
    public class ReportCellsAssertions : GenericCollectionAssertions<IEnumerable<ReportCell>>
    {
        public ReportCellsAssertions(IEnumerable<IEnumerable<ReportCell>> actualValue)
            : base(actualValue)
        {
        }

        protected override string Identifier => "report cells";

        public AndConstraint<ReportCellsAssertions> Equal(IEnumerable<IEnumerable<ReportCell>> expected)
        {
            ReportCell[][] actualCells = this.Subject.Clone();
            ReportCell[][] expectedCells = expected.Clone();

            actualCells.Should().HaveSameCount(expectedCells, "report should have correct count of rows");

            for (int i = 0; i < actualCells.Length; i++)
            {
                actualCells[i].Should().SatisfyRespectively(
                    expectedCells[i].Select(ReportCellHelper.GetCellInspector).ToArray(),
                    $"row at index {i} should contain correct cells");
            }

            return new AndConstraint<ReportCellsAssertions>(this);
        }
    }
}
