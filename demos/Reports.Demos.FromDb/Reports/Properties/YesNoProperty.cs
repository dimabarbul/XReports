using Reports.Excel.Models;
using Reports.Html.Enums;
using Reports.Html.Models;
using Reports.Interfaces;
using Reports.Models;
using Reports.PropertyHandlers;

namespace Reports.Demos.FromDb.Reports.Properties
{
    public class YesNoProperty : ReportCellProperty
    {
        public string YesText => "Yes";
        public string NoText => "No";
    }

    public class HtmlYesNoPropertyHandler : SingleTypePropertyHandler<YesNoProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(YesNoProperty property, HtmlReportCell cell)
        {
            cell.Html = cell.GetValue<bool>() ? property.YesText : property.NoText;
        }
    }

    public class ExcelYesNoPropertyHandler : SingleTypePropertyHandler<YesNoProperty, ExcelReportCell>
    {
        protected override void HandleProperty(YesNoProperty property, ExcelReportCell cell)
        {
            cell.InternalValue = cell.GetValue<bool>() ? property.YesText : property.NoText;
        }
    }
}
