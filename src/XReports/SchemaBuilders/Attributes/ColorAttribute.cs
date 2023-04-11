using System.Drawing;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies cells color.
    /// </summary>
    public sealed class ColorAttribute : BasePropertyAttribute
    {
#pragma warning disable CA1019 // Color cannot be a type of attribute constructor argument
#if NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorAttribute"/> class.
        /// </summary>
        /// <param name="fontColor">Font known color.</param>
        public ColorAttribute(KnownColor fontColor)
            : this(
                Color.FromKnownColor(fontColor),
                null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorAttribute"/> class.
        /// </summary>
        /// <param name="fontColor">Font known color.</param>
        /// <param name="backgroundColor">Background known color.</param>
        public ColorAttribute(KnownColor fontColor, KnownColor backgroundColor)
            : this(
                Color.FromKnownColor(fontColor),
                Color.FromKnownColor(backgroundColor))
        {
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorAttribute"/> class.
        /// </summary>
        /// <param name="fontColor">Font color name.</param>
        /// <param name="backgroundColor">Background color name.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorAttribute"/> class.
        /// </summary>
        /// <param name="fontColor">Font color code.</param>
        public ColorAttribute(int fontColor)
            : this(
                FromColorNumber(fontColor),
                null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorAttribute"/> class.
        /// </summary>
        /// <param name="fontColor">Font color code.</param>
        /// <param name="backgroundColor">Background color code.</param>
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

        /// <summary>
        /// Gets font color.
        /// </summary>
        public Color? FontColor { get; }

        /// <summary>
        /// Gets background color.
        /// </summary>
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
