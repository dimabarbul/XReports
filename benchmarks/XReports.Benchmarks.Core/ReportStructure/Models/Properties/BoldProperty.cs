namespace XReports.Benchmarks.Core.ReportStructure.Models.Properties;

public class BoldProperty : ReportCellsSourceProperty
{
    public override bool Equals(object? obj)
    {
        return obj is BoldProperty;
    }

    public override int GetHashCode()
    {
        return this.GetType().GetHashCode();
    }
}
