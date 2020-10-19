using Reports.Html.Enums;
using Reports.Html.Models;
using Reports.Interfaces;

namespace Reports.Html.Interfaces
{
    public interface IHtmlPropertyHandler
    {
        void Handle(IReportCellProperty property, HtmlReportTableCell cell);
        HtmlPropertyHandlerPriority Priority { get; }
    }
}
