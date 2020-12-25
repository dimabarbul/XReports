namespace Reports.Attributes
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
