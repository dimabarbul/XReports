using System.Globalization;
using XReports.Converter;
using XReports.Html;

namespace XReports.Benchmarks.NewVersion.XReportsProperties;

public class CustomFormatPropertyHtmlHandler : PropertyHandler<CustomFormatProperty, HtmlReportCell>
{
    protected override void HandleProperty(CustomFormatProperty property, HtmlReportCell cell)
    {
        decimal value = cell.GetValue<decimal>();
        string format = value == 100m ? "F0" : "F2";

        cell.SetValue(value.ToString(format, CultureInfo.CurrentCulture));
    }
}
