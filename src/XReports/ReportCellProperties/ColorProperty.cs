using System.Drawing;
using XReports.Table;

namespace XReports.ReportCellProperties
{
    /// <summary>
    /// Property to mark cells which content should be highlighted using color.
    /// </summary>
    public class ColorProperty : ReportCellProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorProperty" /> class.
        /// </summary>
        /// <param name="fontColor">Font color.</param>
        public ColorProperty(Color fontColor)
            : this(fontColor, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorProperty" /> class.
        /// </summary>
        /// <param name="fontColor">Font color.</param>
        /// <param name="backgroundColor">Background color.</param>
        public ColorProperty(Color? fontColor, Color? backgroundColor)
        {
            this.FontColor = fontColor;
            this.BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Gets font color.
        /// </summary>
        public Color? FontColor { get; }

        /// <summary>
        /// Gets background color.
        /// </summary>
        public Color? BackgroundColor { get; }
    }
}
