using System.Text;
using XReports.Html.Writers;

namespace XReports.Demos.FromDb.XReports
{
    public class MyHtmlStringWriter : HtmlStringWriter
    {
        public MyHtmlStringWriter(IHtmlStringCellWriter htmlStringCellWriter)
            : base(htmlStringCellWriter)
        {
        }

        protected override void BeginTable(StringBuilder stringBuilder)
        {
            stringBuilder.Append(@"<table class=""table table-sm"">");
        }
    }
}
