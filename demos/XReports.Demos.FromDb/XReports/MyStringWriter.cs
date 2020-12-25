using System.Threading.Tasks;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.Demos.FromDb.XReports
{
    public class MyStringWriter : StringWriter
    {
        public MyStringWriter(IStringCellWriter stringCellWriter)
            : base(stringCellWriter)
        {
        }

        protected override async Task BeginTableAsync()
        {
            await this.WriteTextAsync(@"<table class=""table table-sm"">");
        }
    }
}
