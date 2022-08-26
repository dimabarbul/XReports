namespace XReports.Benchmarks.Core.ReportStructure.Models.Properties;

public class DecimalPrecisionProperty : ReportCellsSourceProperty
{
    public int Precision { get; }

    public DecimalPrecisionProperty(int precision)
    {
        this.Precision = precision;
    }

    public override bool Equals(object? obj)
    {
        return obj is DecimalPrecisionProperty decimalPrecisionProperty
            && this.Precision == decimalPrecisionProperty.Precision;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Precision);
    }
}
