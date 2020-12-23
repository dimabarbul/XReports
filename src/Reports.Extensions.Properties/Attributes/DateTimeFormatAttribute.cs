using Reports.Extensions.AttributeBasedBuilder.Attributes;

namespace Reports.Extensions.Properties.Attributes
{
    public class DateTimeFormatAttribute : AttributeBase
    {
        public string Format { get; }

        public DateTimeFormatAttribute(string format)
        {
            this.Format = format;
        }
    }
}
