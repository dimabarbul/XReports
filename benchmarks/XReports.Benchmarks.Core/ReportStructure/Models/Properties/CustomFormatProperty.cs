namespace XReports.Benchmarks.Core.ReportStructure.Models.Properties;

public class CustomFormatProperty : ReportCellsSourceProperty
{
    public override bool Equals(object obj)
    {
        return obj is CustomFormatProperty;
    }

    public override int GetHashCode()
    {
        return this.GetType().GetHashCode();
    }
}
