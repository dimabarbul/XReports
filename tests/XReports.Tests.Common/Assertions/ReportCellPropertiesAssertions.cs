using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using XReports.Table;
using XReports.Tests.Common.Helpers;

namespace XReports.Tests.Common.Assertions
{
    public class ReportCellPropertiesAssertions : GenericCollectionAssertions<IReportCellProperty>
    {
        public ReportCellPropertiesAssertions(IEnumerable<IReportCellProperty> actualValue)
            : base(actualValue)
        {
        }

        protected override string Identifier => "report cell properties";

        public AndConstraint<ReportCellPropertiesAssertions> BeEquivalentTo(IEnumerable<IReportCellProperty> expected)
        {
            Execute.Assertion
                .ForCondition(ReportCellHelper.AreObjectCollectionsShallowlyEquivalent(this.Subject, expected))
                .FailWith("Expected cell properties {0} to be equivalent to {1}", this.Subject, expected);

            return new AndConstraint<ReportCellPropertiesAssertions>(this);
        }
    }
}
