namespace XReports.BenchmarksCore.ReportStructure.Models.Properties;

public class SameColumnFormatProperty : ReportCellsSourceProperty
{
    public override bool Equals(object? obj)
    {
        return obj is SameColumnFormatProperty;
    }

    public override int GetHashCode()
    {
        return this.GetType().GetHashCode();
    }
}
