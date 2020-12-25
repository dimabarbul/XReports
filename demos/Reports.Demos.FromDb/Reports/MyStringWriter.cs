using System.Threading.Tasks;
using Reports.Interfaces;
using Reports.Writers;

namespace Reports.Demos.FromDb.Reports
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
