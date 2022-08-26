using System.Linq;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Html.Tests
{
    public partial class HtmlReportConverterTest
    {
        private HtmlReportCell[][] GetHeaderCellsAsArray(IReportTable<HtmlReportCell> table)
        {
            return table.HeaderRows.Select(row => row.ToArray()).ToArray();
        }

        private HtmlReportCell[][] GetBodyCellsAsArray(IReportTable<HtmlReportCell> table)
        {
            return table.Rows.Select(row => row.ToArray()).ToArray();
        }

        private ReportCell CreateReportCell<TValue>(TValue title, params ReportCellProperty[] properties)
        {
            ReportCell cell = ReportCell.FromValue(title);

            foreach (ReportCellProperty property in properties)
            {
                cell.AddProperty(property);
            }

            return cell;
        }
    }
}
