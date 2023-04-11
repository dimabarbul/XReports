using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="AlignmentProperty"/> during conversion to Excel.
    /// </summary>
    public class AlignmentPropertyExcelHandler : PropertyHandler<AlignmentProperty, ExcelReportCell>
    {
        /// <inheritdoc />
        protected override void HandleProperty(AlignmentProperty property, ExcelReportCell cell)
        {
            cell.HorizontalAlignment = property.Alignment;
        }
    }
}
