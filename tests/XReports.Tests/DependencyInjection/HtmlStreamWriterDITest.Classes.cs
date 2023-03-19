using System;
using System.IO;
using System.Threading.Tasks;
using XReports.Html;
using XReports.Html.Writers;
using XReports.Table;

namespace XReports.Tests.DependencyInjection
{
    public partial class HtmlStreamWriterDITest
    {
        private class CustomHtmlStreamWriter : IHtmlStreamWriter
        {
            public Task WriteAsync(IReportTable<HtmlReportCell> reportTable, Stream stream)
            {
                throw new NotImplementedException();
            }

            public Task WriteAsync(IReportTable<HtmlReportCell> reportTable, StreamWriter streamWriter)
            {
                throw new NotImplementedException();
            }
        }

        private class CustomHtmlStreamCellWriter : IHtmlStreamCellWriter
        {
            public Task WriteHeaderCellAsync(StreamWriter streamWriter, HtmlReportCell cell)
            {
                throw new NotImplementedException();
            }

            public Task WriteBodyCellAsync(StreamWriter streamWriter, HtmlReportCell cell)
            {
                throw new NotImplementedException();
            }
        }
    }
}
