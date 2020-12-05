using Reports.Core.Enums;
using Reports.Core.Models;
using Reports.Core.PropertyHandlers;

namespace Reports.Demos.FromDb.Reports.Properties
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
            cell.Html = cell.GetValue<bool>() ? property.YesText : property.NoText;
        }
    }

    public class YesNoPropertyExcelHandler : PropertyHandler<YesNoProperty, ExcelReportCell>, IExcelHandler
    {
        protected override void HandleProperty(YesNoProperty property, ExcelReportCell cell)
        {
            cell.InternalValue = cell.GetValue<bool>() ? property.YesText : property.NoText;
        }
    }
}
