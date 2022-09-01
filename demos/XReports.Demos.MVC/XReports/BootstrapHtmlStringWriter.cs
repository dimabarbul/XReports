using XReports.Interfaces;
using XReports.Writers;

namespace XReports.Demos.MVC.XReports
{
    public class BootstrapHtmlStringWriter : HtmlStringWriter
    {
        public BootstrapHtmlStringWriter(IHtmlStringCellWriter htmlStringCellWriter)
            : base(htmlStringCellWriter)
        {
        }

        protected override void BeginTable()
        {
            this.StringBuilder.Append(@"<table class=""table table-sm"">");
        }

        protected override void BeginHead()
        {
            this.StringBuilder.Append(@"<thead class=""thead-dark"">");
        }
    }
}
