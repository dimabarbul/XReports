using System;
using Reports.ValueFormatOptions;

namespace Reports.Models.Cells.Simple
{
    public class DecimalReportCell : ReportCell<decimal>
    {
        public override string DisplayValue => this.Value.ToString(this.GetValueFormatString());

        protected DecimalFormatOptions ValueFormatOptions { get; set; }

        public override void SetValueFormatOptions(object formatOptions)
        {
            this.ValueFormatOptions = formatOptions as DecimalFormatOptions ?? throw new ArgumentException();
        }

        private string GetValueFormatString()
        {
            return $"N{this.ValueFormatOptions.DecimalPlaces}";
        }
    }
}
