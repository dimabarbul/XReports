using System.Collections.Generic;
using FluentAssertions;
using XReports.Enums;
using XReports.Interfaces;
using XReports.Models;
using XReports.PropertyHandlers;
using Xunit;

namespace XReports.Html.Tests
{
    public partial class HtmlReportConverterTest
    {
        [Fact]
        public void Build_SeveralHandlers_BothApplied()
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>()
            {
                HeaderRows = new List<IEnumerable<ReportCell>>()
                {
                    new ReportCell[] { ReportCell.FromValue("Value"), },
                },
                Rows = new List<IEnumerable<ReportCell>>()
                {
                    new ReportCell[]
                    {
                        this.CreateReportCell("Test", new BoldProperty()),
                    },
                },
            };

            ReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(GetCustomPropertyHandlers());
            IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(table);

            HtmlReportCell[][] cells = this.GetBodyCellsAsArray(htmlReportTable);
            cells.Should().HaveCount(1);
            cells[0][0].GetValue<string>().Should().Be("bold: TEST");
        }

        private static IEnumerable<IPropertyHandler<HtmlReportCell>> GetCustomPropertyHandlers()
        {
            return new IPropertyHandler<HtmlReportCell>[]
            {
                new BoldToUpperHandler(),
                new BoldMarkedWithTextHandler(),
            };
        }

        private class BoldProperty : ReportCellProperty
        {
        }

        private class BoldToUpperHandler : PropertyHandler<BoldProperty, HtmlReportCell>
        {
            public override int Priority => (int)HtmlPropertyHandlerPriority.Text;

            protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
            {
                string value = cell.GetValue<string>();
                cell.SetValue(value.ToUpperInvariant());
            }
        }

        private class BoldMarkedWithTextHandler : PropertyHandler<BoldProperty, HtmlReportCell>
        {
            public override int Priority => (int)HtmlPropertyHandlerPriority.Text;

            protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
            {
                string value = cell.GetValue<string>();
                cell.SetValue($"bold: {value}");
            }
        }
    }
}
