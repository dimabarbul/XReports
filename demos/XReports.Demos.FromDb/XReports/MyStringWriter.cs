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

        protected override void BeginTable()
        {
            this.stringBuilder.Append(@"<table class=""table table-sm"">");
        }
    }
}
