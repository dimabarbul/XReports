using XReports.Converter;
using XReports.Models;

namespace XReports.Demos.FromDb.XReports.Properties
{
    public class YesNoPropertyExcelHandler : PropertyHandler<YesNoProperty, ExcelReportCell>, IExcelHandler
    {
        protected override void HandleProperty(YesNoProperty property, ExcelReportCell cell)
        {
            cell.SetValue(cell.GetValue<bool>() ? property.YesText : property.NoText);
        }
    }
}
