using System.Threading.Tasks;
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
            this.stringBuilder.Append(@"<table class=""table table-sm"">");
        }
    }
}
