using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;

namespace XReports.Core.Tests.Assertions
{
    internal class NewReportCellsAssertions : GenericCollectionAssertions<IEnumerable<ReportConverterTest.NewReportCell>>
    {
        public NewReportCellsAssertions(IEnumerable<IEnumerable<ReportConverterTest.NewReportCell>> actualValue)
            : base(actualValue)
        {
        }

        protected override string Identifier => "report cells";

        public AndConstraint<NewReportCellsAssertions> Equal(IEnumerable<IEnumerable<ReportConverterTest.NewReportCell>> expected)
        {
            ReportConverterTest.NewReportCell[][] actualCells = this.Subject.Clone();
            ReportConverterTest.NewReportCell[][] expectedCells = expected.Clone();

            actualCells.Should().HaveSameCount(expectedCells, "report should have correct count of rows");

            for (int i = 0; i < actualCells.Length; i++)
            {
                actualCells[i].Should().SatisfyRespectively(
                    expectedCells[i].Select(this.GetCellInspector).ToArray(),
                    $"row at index {i} should contain correct cells");
            }

            return new AndConstraint<NewReportCellsAssertions>(this);
        }

        private Action<ReportConverterTest.NewReportCell> GetCellInspector(ReportConverterTest.NewReportCell expectedCell)
        {
            if (expectedCell == null)
            {
                return actual => actual.Should().BeNull();
            }

            return actual =>
            {
                ReportCellHelper.GetCellInspector(expectedCell).Invoke(actual);
                actual.Data.Should().BeEquivalentTo(expectedCell.Data);
            };
        }
    }
}
