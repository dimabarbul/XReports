using System;
using XReports.Table;

namespace XReports.ReportCellProperties
{
    /// <summary>
    /// Property to mark cells which value should have maximum length.
    /// </summary>
    public class MaxLengthProperty : IReportCellProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaxLengthProperty"/> class.
        /// </summary>
        /// <param name="maxLength">Maximum length of cell value.</param>
        /// <param name="text">Text to append if cell value length is greater that maximum allowed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when maximum length is less or equal to zero.</exception>
        /// <exception cref="ArgumentException">Thrown when text to append is longer or of the same length as maximum length.</exception>
        public MaxLengthProperty(int maxLength, string text = "…")
        {
            if (maxLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length cannot be negative");
            }

            if (text?.Length >= maxLength)
            {
                throw new ArgumentException("Appended text length should be less than maximum length.");
            }

            this.MaxLength = maxLength;
            this.Text = text ?? string.Empty;
        }

        /// <summary>
        /// Gets maximum length.
        /// </summary>
        public int MaxLength { get; }

        /// <summary>
        /// Gets text to append if cell value length is greater that maximum allowed.
        /// </summary>
        public string Text { get; }
    }
}
