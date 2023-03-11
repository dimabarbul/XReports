using System.Drawing;
using XReports.Table;

namespace XReports.Properties
{
    public class ColorProperty : ReportCellProperty
    {
        public ColorProperty(Color fontColor)
            : this(fontColor, null)
        {
        }

        public ColorProperty(Color? fontColor, Color? backgroundColor)
        {
            this.FontColor = fontColor;
            this.BackgroundColor = backgroundColor;
        }

        public Color? FontColor { get; }

        public Color? BackgroundColor { get; }
    }
}
