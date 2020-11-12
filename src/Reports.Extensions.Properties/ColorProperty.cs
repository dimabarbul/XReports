using System.Drawing;
using Reports.Interfaces;

namespace Reports.Extensions.Properties
{
    public class ColorProperty : IReportCellProperty
    {
        public Color? FontColor { get; }
        public Color? BackgroundColor { get; }

        public ColorProperty(Color? fontColor, Color? backgroundColor)
        {
            this.FontColor = fontColor;
            this.BackgroundColor = backgroundColor;
        }
    }
}
