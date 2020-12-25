namespace Reports.Attributes
{
    public class PercentFormatAttribute : AttributeBase
    {
        public int Precision { get; }
        public string PostfixText { get; set; }

        public PercentFormatAttribute(int precision)
        {
            this.Precision = precision;
        }
    }
}
