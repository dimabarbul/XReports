using System.Threading.Tasks;
using Reports.Html.StringWriter;

namespace Reports.Demos.MVC.Reports
{
    public class BootstrapStringWriter : StringWriter
    {
        protected override async Task BeginTableAsync()
        {
            await this.WriteTextAsync(@"<table class=""table table-sm"">");
        }
    }
}
