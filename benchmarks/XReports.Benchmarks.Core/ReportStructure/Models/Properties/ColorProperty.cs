using System.Drawing;

namespace XReports.Benchmarks.Core.ReportStructure.Models.Properties;

public class ColorProperty : ReportCellsSourceProperty
{
    public Color ForegroundColor { get; }
    public Color? BackgroundColor { get; }

    public ColorProperty(Color foregroundColor, Color? backgroundColor = null)
    {
        this.ForegroundColor = foregroundColor;
        this.BackgroundColor = backgroundColor;
    }

    public override bool Equals(object obj)
    {
        return obj is ColorProperty colorProperty
            && this.ForegroundColor == colorProperty.ForegroundColor
            && this.BackgroundColor == colorProperty.BackgroundColor;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.ForegroundColor, this.BackgroundColor);
    }
}
