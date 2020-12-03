using Reports.Enums;
using Reports.Models;
using Reports.PropertyHandlers;

namespace Reports.Demos.FromDb.Reports.Properties
{
    public class YesNoProperty : ReportCellProperty
    {
        public string YesText => "Yes";
        public string NoText => "No";
    }

    public class HtmlYesNoPropertyHandler : PropertyHandler<YesNoProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(YesNoProperty property, HtmlReportCell cell)
        {
            cell.Html = cell.GetValue<bool>() ? property.YesText : property.NoText;
        }
    }

    public class ExcelYesNoPropertyHandler : PropertyHandler<YesNoProperty, ExcelReportCell>
    {
        protected override void HandleProperty(YesNoProperty property, ExcelReportCell cell)
        {
            cell.InternalValue = cell.GetValue<bool>() ? property.YesText : property.NoText;
        }
    }
}
