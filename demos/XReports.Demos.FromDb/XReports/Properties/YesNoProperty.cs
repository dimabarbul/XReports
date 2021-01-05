using XReports.Enums;
using XReports.Models;
using XReports.PropertyHandlers;

namespace XReports.Demos.FromDb.XReports.Properties
{
    public class YesNoProperty : ReportCellProperty
    {
        public string YesText => "Yes";
        public string NoText => "No";
    }

    public class YesNoPropertyHtmlHandler : PropertyHandler<YesNoProperty, HtmlReportCell>, IHtmlHandler
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(YesNoProperty property, HtmlReportCell cell)
        {
            cell.Value = cell.GetValue<bool>() ? property.YesText : property.NoText;
        }
    }

    public class YesNoPropertyExcelHandler : PropertyHandler<YesNoProperty, ExcelReportCell>, IExcelHandler
    {
        protected override void HandleProperty(YesNoProperty property, ExcelReportCell cell)
        {
            cell.Value = cell.GetValue<bool>() ? property.YesText : property.NoText;
        }
    }
}
