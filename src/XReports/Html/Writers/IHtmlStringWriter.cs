using XReports.Table;

namespace XReports.Html.Writers
{
    public interface IHtmlStringWriter
    {
        string WriteToString(IReportTable<HtmlReportCell> reportTable);
    }
}
