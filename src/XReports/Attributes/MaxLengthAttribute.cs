using System;

namespace XReports.Attributes
{
    public class MaxLengthAttribute : AttributeBase
    {
        public int MaxLength { get; }

        public MaxLengthAttribute(int maxLength)
        {
            if (maxLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length cannot be negative");
            }

            this.MaxLength = maxLength;
        }
    }
}
