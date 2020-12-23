using System.Drawing;
using Reports.Extensions.AttributeBasedBuilder.Attributes;

namespace Reports.Extensions.Properties.Attributes
{
    public class ColorAttribute : AttributeBase
    {
        public Color? FontColor { get; }
        public Color? BackgroundColor { get; }

        public ColorAttribute(Color fontColor)
            : this(fontColor, null)
        {
        }

        public ColorAttribute(Color? fontColor, Color? backgroundColor)
        {
            this.FontColor = fontColor;
            this.BackgroundColor = backgroundColor;
        }
    }
}
