using System.Drawing;
using Reports.Models;

namespace Reports.Extensions.Properties
{
    public class ColorProperty : ReportCellProperty
    {
        public Color? FontColor { get; }
        public Color? BackgroundColor { get; }

        public ColorProperty(Color fontColor)
            : this(fontColor, null)
        {
        }

        public ColorProperty(Color? fontColor, Color? backgroundColor)
        {
            this.FontColor = fontColor;
            this.BackgroundColor = backgroundColor;
        }
    }
}
