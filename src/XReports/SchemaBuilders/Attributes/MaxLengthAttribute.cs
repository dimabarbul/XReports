using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies maximum length for cell value.
    /// </summary>
    public sealed class MaxLengthAttribute : BasePropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaxLengthAttribute"/> class.
        /// </summary>
        /// <param name="maxLength">Maximum length of cell value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when maximum length is less or equal to zero.</exception>
        public MaxLengthAttribute(int maxLength)
        {
            if (maxLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length cannot be negative");
            }

            this.MaxLength = maxLength;
        }

        /// <summary>
        /// Gets maximum length.
        /// </summary>
        public int MaxLength { get; }

        /// <summary>
        /// Gets or sets text to append if cell value length is greater that maximum allowed.
        /// </summary>
        public string Text { get; set; } = "…";
    }
}
