using XReports.Models;
using XReports.PropertyHandlers;

namespace XReports.Demos.FromDb.XReports.Properties
{
    public class YesNoPropertyExcelHandler : PropertyHandler<YesNoProperty, ExcelReportCell>, IExcelHandler
    {
        protected override void HandleProperty(YesNoProperty property, ExcelReportCell cell)
        {
            cell.Value = cell.GetValue<bool>() ? property.YesText : property.NoText;
        }
    }
}
