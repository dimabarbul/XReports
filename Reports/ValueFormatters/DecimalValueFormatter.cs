using System.Globalization;
using Reports.Interfaces;

namespace Reports.ValueFormatters
{
    public class DecimalValueFormatter : IValueFormatter<decimal>
    {
        private readonly int decimalPlaces;

        public DecimalValueFormatter(int decimalPlaces = 0)
        {
            this.decimalPlaces = decimalPlaces;
        }

        public string Format(decimal value)
        {
            return value.ToString($"F{this.decimalPlaces}", CultureInfo.InvariantCulture);
        }
    }
}
