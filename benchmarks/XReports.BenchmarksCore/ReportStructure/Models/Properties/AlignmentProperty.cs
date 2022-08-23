using XReports.BenchmarksCore.ReportStructure.Enums;

namespace XReports.BenchmarksCore.ReportStructure.Models.Properties;

public class AlignmentProperty : ReportCellsSourceProperty
{
    public AlignmentProperty(Alignment alignment)
    {
        this.Alignment = alignment;
    }

    public Alignment Alignment { get; }

    public override bool Equals(object? obj)
    {
        return obj is AlignmentProperty alignmentProperty
            && this.Alignment == alignmentProperty.Alignment;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Alignment);
    }
}
