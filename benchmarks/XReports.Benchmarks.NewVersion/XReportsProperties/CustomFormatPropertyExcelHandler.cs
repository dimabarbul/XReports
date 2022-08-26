using XReports.Models;
using XReports.PropertyHandlers;

namespace XReports.Benchmarks.NewVersion.XReportsProperties;

public class CustomFormatPropertyExcelHandler : PropertyHandler<CustomFormatProperty, ExcelReportCell>
{
    protected override void HandleProperty(CustomFormatProperty property, ExcelReportCell cell)
    {
        cell.NumberFormat = "[=100]0;[<100]0.00";
    }
}