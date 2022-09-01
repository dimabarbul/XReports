using System;
using XReports.Enums;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class MaxLengthPropertyHtmlHandler : PropertyHandler<MaxLengthProperty, HtmlReportCell>
    {
        public override int Priority => (int)HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(MaxLengthProperty property, HtmlReportCell cell)
        {
            string value = cell.GetValue<string>();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length <= property.MaxLength)
            {
                return;
            }

            cell.SetValue(value.Substring(0, property.MaxLength - 1) + '…');
        }
    }
}
