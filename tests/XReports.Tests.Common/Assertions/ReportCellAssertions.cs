using FluentAssertions;
using FluentAssertions.Primitives;
using XReports.Models;

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

        public AndConstraint<ReportCellAssertions> Be(ReportCellData expected)
        {
            if (expected == null)
            {
                this.Subject.Should().BeNull("{0} should be null", this.Identifier);
            }
            else
            {
                this.Subject.Should().NotBeNull("{0} should not be null", this.Identifier);
                this.Subject.GetUnderlyingValue().Should()
                    .Be(expected.Value, "value of {0} should be correct", this.Identifier);
                this.Subject.Properties.Should().ContainSameOrEqualElements(expected.Properties);
                this.Subject.ColumnSpan.Should().Be(expected.ColumnSpan, "column span of {0} should be correct",
                    this.Identifier);
                this.Subject.RowSpan.Should()
                    .Be(expected.RowSpan, "row span of {0} should be correct", this.Identifier);
            }

            return new AndConstraint<ReportCellAssertions>(this);
        }
    }
}
