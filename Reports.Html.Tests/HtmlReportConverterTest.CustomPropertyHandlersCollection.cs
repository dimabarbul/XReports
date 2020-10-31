using System.Collections.Generic;
using FluentAssertions;
using Reports.Html.Enums;
using Reports.Html.Interfaces;
using Reports.Html.Models;
using Reports.Html.PropertyHandlers;
using Reports.Html.PropertyHandlers.StandardHtml;
using Reports.Interfaces;
using Reports.Models;
using Reports.Models.Cells;
using Reports.Models.Properties;
using Xunit;

namespace Reports.Html.Tests
{
    public partial class HtmlReportConverterTest
    {
        [Fact]
        public void Build_SeveralHandlers_BothApplied()
        {
            ReportTable table = new ReportTable
            {
                HeaderCells = new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] { new ReportCell<string>("Value"), },
                },
                Cells = new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] {
                        new ReportCell<string>("Test", new [] { new BoldProperty() }),
                    }
                },
            };

            HtmlReportConverter converter = new HtmlReportConverter(GetCustomPropertyHandlers());
            HtmlReportTable htmlReportTable = converter.Convert(table);

            HtmlReportTableBodyCell[][] cells = this.GetBodyCellsAsArray(htmlReportTable);
            cells.Should().HaveCount(1);
            cells[0][0].Html.Should().Be("<strong>TEST</strong>");
        }

        private static IEnumerable<IHtmlPropertyHandler> GetCustomPropertyHandlers()
        {
            return new IHtmlPropertyHandler[]
            {
                new BoldToUpperHandler(),
                new StandardHtmlBoldPropertyHandler(),
            };
        }

        private class BoldToUpperHandler : HtmlPropertyHandler<BoldProperty>
        {
            protected override void HandleProperty(BoldProperty property, HtmlReportTableCell cell)
            {
                cell.Text = cell.Text.ToUpperInvariant();
            }

            public override HtmlPropertyHandlerPriority Priority => HtmlPropertyHandlerPriority.Text;
        }
    }
}
