using System.Drawing;

namespace XReports.Attributes
{
    public class ColorAttribute : BasePropertyAttribute
    {
        public ColorAttribute(Color fontColor)
            : this(fontColor, null)
        {
        }

        public ColorAttribute(Color? fontColor, Color? backgroundColor)
        {
            this.FontColor = fontColor;
            this.BackgroundColor = backgroundColor;
        }

        public Color? FontColor { get; }

        public Color? BackgroundColor { get; }
    }
}
