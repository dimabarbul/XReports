using System;
using System.ComponentModel;
using XReports.ReportCellProperties;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies cell content alignment.
    /// </summary>
    public sealed class AlignmentAttribute : BasePropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlignmentAttribute"/> class.
        /// </summary>
        /// <param name="alignment">Alignment.</param>
        /// <exception cref="InvalidEnumArgumentException">Thrown when alignment value is not defined in <see cref="XReports.ReportCellProperties.Alignment"/>.</exception>
        public AlignmentAttribute(Alignment alignment)
        {
            if (!Enum.IsDefined(typeof(Alignment), alignment))
            {
                throw new InvalidEnumArgumentException(nameof(alignment), (int)alignment, typeof(Alignment));
            }

            this.Alignment = alignment;
        }

        /// <summary>
        /// Gets alignment.
        /// </summary>
        public Alignment Alignment { get; }
    }
}
