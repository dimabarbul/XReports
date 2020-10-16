using System;
using Reports.ValueFormatOptions;

namespace Reports.Models.Cells.Simple
{
    public class TextReportCell : ReportCell<string>
    {
        public override string DisplayValue => this.Value;

        protected TextFormatOptions ValueFormatOptions { get; set; }

        public override void SetValueFormatOptions(object formatOptions)
        {
            this.ValueFormatOptions = formatOptions as TextFormatOptions ?? throw new ArgumentException();
        }
    }
}
