using XReports.Enums;
using XReports.Models;
using XReports.PropertyHandlers;

namespace XReports.Demos.FromDb.XReports.Properties
{
    public class YesNoPropertyHtmlHandler : PropertyHandler<YesNoProperty, HtmlReportCell>, IHtmlHandler
    {
        public override int Priority => (int)HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(YesNoProperty property, HtmlReportCell cell)
        {
            cell.SetValue(cell.GetValue<bool>() ? property.YesText : property.NoText);
        }
    }
}
