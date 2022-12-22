using System;
using System.Collections.Generic;
using XReports.Models;

namespace XReports.Tests.Common.Assertions
{
    public class ReportCellData
    {
        public ReportCellData(object value)
        {
            this.Value = value;
        }

        public object Value { get; }
        public IReadOnlyCollection<ReportCellProperty> Properties { get; set; } = Array.Empty<ReportCellProperty>();
        public int ColumnSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;
    }
}
