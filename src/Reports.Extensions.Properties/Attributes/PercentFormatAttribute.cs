namespace Reports.Extensions.Properties.Attributes
{
    public class PercentFormatAttribute : AttributeBase
    {
        public int Precision { get; }

        public PercentFormatAttribute(int precision)
        {
            this.Precision = precision;
        }
    }
}
