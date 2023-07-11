using System.Text;
using XReports.Html;
using XReports.Html.Writers;
using XReports.Table;

namespace XReports.Demos.XReports;

public class BootstrapHtmlStringWriter : HtmlStringWriter
{
    public BootstrapHtmlStringWriter(IHtmlStringCellWriter htmlStringCellWriter)
        : base(htmlStringCellWriter)
    {
    }

    protected override void BeginTable(StringBuilder stringBuilder, IReportTable<HtmlReportCell> reportTable)
    {
        stringBuilder.Append(@"<table class=""table table-sm"">");
    }

    protected override void BeginHead(StringBuilder stringBuilder)
    {
        stringBuilder.Append(@"<thead class=""thead-dark"">");
    }
}
