using System.Collections.Generic;
using System.Collections.Immutable;
using XReports.Models;

namespace XReports.Tests.Assertions
{
    internal class ReportCellData
    {
        public ReportCellData(object value)
        {
            this.Value = value;
        }

        public object Value { get; }
        public IReadOnlyCollection<ReportCellProperty> Properties { get; init; } = ImmutableArray<ReportCellProperty>.Empty;
        public int ColumnSpan { get; init; } = 1;
        public int RowSpan { get; init; } = 1;
    }
}
