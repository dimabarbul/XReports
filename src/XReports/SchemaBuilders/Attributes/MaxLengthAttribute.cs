using System;

namespace XReports.SchemaBuilders.Attributes
{
    public sealed class MaxLengthAttribute : BasePropertyAttribute
    {
        public MaxLengthAttribute(int maxLength)
        {
            if (maxLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length cannot be negative");
            }

            this.MaxLength = maxLength;
        }

        public int MaxLength { get; }

        public string Text { get; set; } = "…";
    }
}
