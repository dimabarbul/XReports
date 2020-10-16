using System;

namespace Reports.Models.Cells.Simple
{
    public class IntegerReportCell : ReportCell<int>
    {
        public override string DisplayValue => this.Value.ToString();

        public override void SetValueFormatOptions(object formatOptions)
        {
            throw new InvalidOperationException();
        }
    }
}
