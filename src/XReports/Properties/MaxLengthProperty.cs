using System;
using XReports.Models;

namespace XReports.Properties
{
    public class MaxLengthProperty : ReportCellProperty
    {
        public MaxLengthProperty(int maxLength, string text = "…")
        {
            if (maxLength < 0)
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

        public int MaxLength { get; }

        public string Text { get; }
    }
}
