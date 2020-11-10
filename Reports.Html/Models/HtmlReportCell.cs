using Reports.Models;

namespace Reports.Html.Models
{
    public class HtmlReportCell : BaseReportCell
    {
        public string Html { get; set; }

        public override void Copy(BaseReportCell reportCell)
        {
            base.Copy(reportCell);

            this.Html = reportCell.InternalValue.ToString();
        }
    }
}
