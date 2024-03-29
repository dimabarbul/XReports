using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using XReports.Table;
using XReports.Tests.Common.Helpers;

namespace XReports.Tests.Common.Assertions
{
    public class ReportTablePropertiesAssertions : GenericCollectionAssertions<IReportTableProperty>
    {
        public ReportTablePropertiesAssertions(IEnumerable<IReportTableProperty> actualValue)
            : base(actualValue)
        {
        }

        protected override string Identifier => "report table properties";

        public AndConstraint<ReportTablePropertiesAssertions> BeEquivalentTo(IEnumerable<IReportTableProperty> expected)
        {
            Execute.Assertion
                .ForCondition(ReportCellHelper.AreObjectCollectionsShallowlyEquivalent(this.Subject, expected))
                .FailWith("Expected table properties {0} to be equivalent to {1}", this.Subject, expected);

            return new AndConstraint<ReportTablePropertiesAssertions>(this);
        }
    }
}
