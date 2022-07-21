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
            this.stringBuilder.Append(@"<table class=""table table-sm"">");
        }

        protected override void BeginHead()
        {
            this.stringBuilder.Append(@"<thead class=""thead-dark"">");
        }
    }
}
