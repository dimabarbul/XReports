namespace XReports.Benchmarks.Core.ReportStructure.Models.Properties;

public class DateTimeFormatProperty : ReportCellsSourceProperty
{
    public string Format { get; }

    public DateTimeFormatProperty(string format)
    {
        this.Format = format;
    }

    public override bool Equals(object? obj)
    {
        return obj is DateTimeFormatProperty dateTimeFormatProperty
            && this.Format == dateTimeFormatProperty.Format;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Format);
    }
}
