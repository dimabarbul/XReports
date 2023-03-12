using System;
using System.Text;
using XReports.Html;
using XReports.Html.Writers;
using XReports.Table;

namespace XReports.Tests.DependencyInjection
{
    public partial class HtmlStringWriterDITest
    {
        private class CustomHtmlStringWriter : IHtmlStringWriter
        {
            public string WriteToString(IReportTable<HtmlReportCell> reportTable)
            {
                throw new NotImplementedException();
            }
        }

        private class CustomHtmlStringCellWriter : IHtmlStringCellWriter
        {
            public void WriteHeaderCell(StringBuilder stringBuilder, HtmlReportCell cell)
            {
                throw new NotImplementedException();
            }

            public void WriteBodyCell(StringBuilder stringBuilder, HtmlReportCell cell)
            {
                throw new NotImplementedException();
            }
        }
    }
}
