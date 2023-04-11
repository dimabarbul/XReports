using System;
using System.ComponentModel;
using XReports.Table;

namespace XReports.ReportCellProperties
{
    /// <summary>
    /// Property to mark cells with content aligned in certain way.
    /// </summary>
    public class AlignmentProperty : ReportCellProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlignmentProperty" /> class.
        /// </summary>
        /// <param name="alignment">Alignment.</param>
        /// <exception cref="InvalidEnumArgumentException">Alignment is invalid.</exception>
        public AlignmentProperty(Alignment alignment)
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
