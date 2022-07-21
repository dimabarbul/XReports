using XReports.Interfaces;
using XReports.Writers;

namespace XReports.Demos.MVC.XReports
{
    public class BootstrapStringWriter : StringWriter
    {
        public BootstrapStringWriter(IStringCellWriter stringCellWriter)
            : base(stringCellWriter)
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
