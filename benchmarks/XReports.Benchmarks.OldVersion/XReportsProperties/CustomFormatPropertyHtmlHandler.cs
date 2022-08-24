using XReports.Models;
using XReports.PropertyHandlers;

namespace XReports.Benchmarks.OldVersion.XReportsProperties;

public class CustomFormatPropertyHtmlHandler : PropertyHandler<CustomFormatProperty, HtmlReportCell>
{
    protected override void HandleProperty(CustomFormatProperty property, HtmlReportCell cell)
    {
        decimal value = cell.GetValue<decimal>();
        string format = value == 100m ? "F0" : "F2";

        cell.Value = value.ToString(format);
    }
}
