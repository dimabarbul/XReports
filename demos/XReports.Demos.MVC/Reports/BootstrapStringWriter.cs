using System.Threading.Tasks;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.Demos.MVC.Reports
{
    public class BootstrapStringWriter : StringWriter
    {
        public BootstrapStringWriter(IStringCellWriter stringCellWriter)
            : base(stringCellWriter)
        {
        }

        protected override async Task BeginTableAsync()
        {
            await this.WriteTextAsync(@"<table class=""table table-sm"">");
        }

        protected override async Task BeginHeadAsync()
        {
            await this.WriteTextAsync(@"<thead class=""thead-dark"">");
        }
    }
}
