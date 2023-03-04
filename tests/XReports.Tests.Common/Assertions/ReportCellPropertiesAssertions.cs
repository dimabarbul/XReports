using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using XReports.Models;
using XReports.Tests.Common.Helpers;

namespace XReports.Tests.Common.Assertions
{
    public class ReportCellPropertiesAssertions : GenericCollectionAssertions<ReportCellProperty>
    {
        public ReportCellPropertiesAssertions(IEnumerable<ReportCellProperty> actualValue)
            : base(actualValue)
        {
        }

        protected override string Identifier => "report cell properties";

        public AndConstraint<ReportCellPropertiesAssertions> BeEquivalentTo(IEnumerable<ReportCellProperty> expected)
        {
            Execute.Assertion
                .ForCondition(ReportCellHelper.AreObjectCollectionsShallowlyEquivalent(this.Subject, expected))
                .FailWith("Expected cell properties {0} to be equivalent to {1}", this.Subject, expected);

            return new AndConstraint<ReportCellPropertiesAssertions>(this);
        }
    }
}
