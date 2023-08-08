using XReports.Converter;
using XReports.Table;

namespace XReports.Core.Tests.DependencyInjection
{
    public partial class XReportsDITest
    {
        private class HtmlCell : ReportCell
        {
        }

        private interface IHtmlPropertyHandler : IPropertyHandler<HtmlCell>
        {
        }

        private class HtmlCellHandlerCustomInterface : IHtmlPropertyHandler
        {
            public int Priority => 0;

            public bool Handle(IReportCellProperty property, HtmlCell cell)
            {
                return false;
            }
        }

        private class HtmlCellAnotherHandlerCustomInterface : IHtmlPropertyHandler
        {
            public int Priority => 0;

            public bool Handle(IReportCellProperty property, HtmlCell cell)
            {
                return false;
            }
        }

        private class Dependency
        {
        }

        private class HandlerWithDependency : IPropertyHandler<HtmlCell>
        {
            public int Priority => 0;

            public HandlerWithDependency(Dependency _)
            {
            }

            public bool Handle(IReportCellProperty property, HtmlCell cell)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
