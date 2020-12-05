namespace Reports.Extensions.Properties.Attributes
{
    public class DecimalFormatAttribute : AttributeBase
    {
        public int Precision { get; }
        public bool CultureAgnostic { get; set; }

        public DecimalFormatAttribute(int precision)
        {
            this.Precision = precision;
        }
    }
}
