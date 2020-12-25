using System.Linq;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Html.Tests
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
            ReportCell<TValue> cell = new ReportCell<TValue>(title);

            foreach (ReportCellProperty property in properties)
            {
                cell.AddProperty(property);
            }

            return cell;
        }
    }
}
