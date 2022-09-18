using XReports.Interfaces;
using XReports.Models;

namespace XReports.Core.Tests.Options
{
    public partial class ReportConverterOptionsTests
    {
        private class HtmlCell : BaseReportCell
        {
        }

        private class HtmlHandler : IPropertyHandler<HtmlCell>
        {
            public int Priority => 0;

            public bool Handle(ReportCellProperty property, HtmlCell cell)
            {
                throw new System.NotImplementedException();
            }
        }

        private class AnotherHtmlHandler : IPropertyHandler<HtmlCell>
        {
            public int Priority => 0;

            public bool Handle(ReportCellProperty property, HtmlCell cell)
            {
                throw new System.NotImplementedException();
            }
        }

        private abstract class AbstractHtmlHandler : IPropertyHandler<HtmlCell>
        {
            public abstract int Priority { get; }
            public abstract bool Handle(ReportCellProperty property, HtmlCell cell);
        }

        private class ReportCellHandler : IPropertyHandler<ReportCell>
        {
            public int Priority => 0;

            public bool Handle(ReportCellProperty property, ReportCell cell)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
