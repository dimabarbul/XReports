using XReports.Interfaces;
using XReports.Writers;

namespace XReports.Demos.FromDb.XReports
{
    public class MyHtmlStringWriter : HtmlStringWriter
    {
        public MyHtmlStringWriter(IHtmlStringCellWriter htmlStringCellWriter)
            : base(htmlStringCellWriter)
        {
        }

        protected override void BeginTable()
        {
            this.StringBuilder.Append(@"<table class=""table table-sm"">");
        }
    }
}
