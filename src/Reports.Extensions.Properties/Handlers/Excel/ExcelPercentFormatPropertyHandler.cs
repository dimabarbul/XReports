using System.Linq;
using Reports.Excel.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.Handlers.Excel
{
    public class ExcelPercentFormatPropertyHandler : SingleTypePropertyHandler<PercentFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(PercentFormatProperty property, ExcelReportCell cell)
        {
            cell.NumberFormat = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))} %";
        }
    }
}
