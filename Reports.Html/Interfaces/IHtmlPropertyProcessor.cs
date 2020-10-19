using Reports.Html.Models;
using Reports.Interfaces;

namespace Reports.Html.Interfaces
{
    public interface IHtmlPropertyProcessor
    {
        void ProcessProperties(IReportCell cell, HtmlReportTableCell htmlCell);
    }
}
