using Reports.Models;

namespace Reports.Html.Models
{
    public class HtmlReportCell : BaseReportCell
    {
        public string Html { get; set; }

        public override void CopyFrom(BaseReportCell reportCell)
        {
            base.CopyFrom(reportCell);

            this.Html = reportCell.InternalValue.ToString();
        }
    }
}
