using System;

namespace Reports.Models.Cells
{
    public class HeaderReportCell : ReportCell<string>
    {
        public override string DisplayValue { get; }

        public HeaderReportCell(string text)
        {
            this.DisplayValue = text;
        }

        public override void SetValueFormatOptions(object formatOptions)
        {
            throw new InvalidOperationException();
        }
    }
}
