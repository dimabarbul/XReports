using System.Collections.Generic;
using Reports.Models;

namespace Reports.Html.Models
{
    public class HtmlReportCell : BaseReportCell
    {
        public string Html { get; set; }

        #region Most used or complex HTML attributes
        public HashSet<string> CssClasses { get; set; } = new HashSet<string>();
        public Dictionary<string, string> Styles { get; set; }
        #endregion

        public Dictionary<string, string> Attributes { get; set; }

        public override void CopyFrom(BaseReportCell reportCell)
        {
            base.CopyFrom(reportCell);

            this.Html = reportCell.InternalValue.ToString();
        }
    }
}
