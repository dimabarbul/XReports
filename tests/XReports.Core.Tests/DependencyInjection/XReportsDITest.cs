using XReports.Interfaces;
using XReports.Models;

namespace XReports.Core.Tests.DependencyInjection
{
    public partial class XReportsDITest
    {
        private class HtmlCell : BaseReportCell
        {
        }

        private interface IHtmlPropertyHandler : IPropertyHandler<HtmlCell>
        {
        }

        private class HtmlCellHandler : IPropertyHandler<HtmlCell>
        {
            public int Priority => 0;

            public void Handle(ReportCellProperty property, HtmlCell cell)
            {
            }
        }

        private class HtmlCellHandlerCustomInterface : IHtmlPropertyHandler
        {
            public int Priority => 0;

            public void Handle(ReportCellProperty property, HtmlCell cell)
            {
            }
        }

        private class HtmlCellAnotherHandlerCustomInterface : IHtmlPropertyHandler
        {
            public int Priority => 0;

            public void Handle(ReportCellProperty property, HtmlCell cell)
            {
            }
        }
    }
}
