using System.Drawing;

namespace XReports.SchemaBuilders.Attributes
{
    public sealed class ColorAttribute : BasePropertyAttribute
    {
#pragma warning disable CA1019 // Color cannot be a type of attribute constructor argument
#if NETSTANDARD2_1_OR_GREATER
        public ColorAttribute(KnownColor fontColor)
            : this(
                Color.FromKnownColor(fontColor),
                null)
        {
        }

        public ColorAttribute(KnownColor fontColor, KnownColor backgroundColor)
            : this(
                Color.FromKnownColor(fontColor),
                Color.FromKnownColor(backgroundColor))
        {
        }
#endif

        public ColorAttribute(string fontColor, string backgroundColor = null)
            : this(
                string.IsNullOrWhiteSpace(fontColor) ?
                    (Color?)null :
                    Color.FromName(fontColor),
                string.IsNullOrWhiteSpace(backgroundColor) ?
                    (Color?)null :
                    Color.FromName(backgroundColor))
        {
        }

        public ColorAttribute(int fontColor)
            : this(
                FromColorNumber(fontColor),
                null)
        {
        }

        public ColorAttribute(int fontColor, int backgroundColor)
            : this(
                FromColorNumber(fontColor),
                FromColorNumber(backgroundColor))
        {
        }

#pragma warning restore CA1019

        private ColorAttribute(Color? fontColor, Color? backgroundColor)
        {
            this.FontColor = fontColor;
            this.BackgroundColor = backgroundColor;
        }

        public Color? FontColor { get; }

        public Color? BackgroundColor { get; }

        private static Color FromColorNumber(int fontColor)
        {
            return Color.FromArgb(
                (fontColor >> 16) & 0xFF,
                (fontColor >> 8) & 0xFF,
                fontColor & 0xFF);
        }
    }
}
