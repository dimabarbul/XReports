using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies display format for cells with DateTime value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class DateTimeFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeFormatAttribute"/> class.
        /// </summary>
        /// <param name="format">Format to apply.</param>
        /// <exception cref="ArgumentException">Thrown when format is null or empty.</exception>
        public DateTimeFormatAttribute(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format cannot be empty or null", nameof(format));
            }

            this.Format = format;
        }

        /// <summary>
        /// Gets format to apply.
        /// </summary>
        public string Format { get; }
    }
}
