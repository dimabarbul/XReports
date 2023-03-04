using FluentAssertions;
using FluentAssertions.Primitives;
using XReports.Models;
using XReports.Tests.Common.Helpers;

namespace XReports.Tests.Common.Assertions
{
    public class ReportCellAssertions : ReferenceTypeAssertions<ReportCell, ReportCellAssertions>
    {
        public ReportCellAssertions(ReportCell actualValue)
            : base(actualValue)
        {
        }

        public ReportCellAssertions(ReportCell actualValue, string identifier)
            : base(actualValue)
        {
            this.Identifier = identifier;
        }

        protected override string Identifier { get; } = "report cell";

        public AndConstraint<ReportCellAssertions> Equal(ReportCell expected)
        {
            ReportCellHelper.GetCellInspector(expected).Invoke(this.Subject);

            return new AndConstraint<ReportCellAssertions>(this);
        }
    }
}
