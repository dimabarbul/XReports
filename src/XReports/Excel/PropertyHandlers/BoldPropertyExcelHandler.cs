using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="BoldProperty"/> during conversion to Excel.
    /// </summary>
    public class BoldPropertyExcelHandler : PropertyHandler<BoldProperty, ExcelReportCell>
    {
        /// <inheritdoc />
        protected override void HandleProperty(BoldProperty property, ExcelReportCell cell)
        {
            cell.IsBold = true;
        }
    }
}
