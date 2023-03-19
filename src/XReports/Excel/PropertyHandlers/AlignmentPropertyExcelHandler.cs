using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
{
    public class AlignmentPropertyExcelHandler : PropertyHandler<AlignmentProperty, ExcelReportCell>
    {
        protected override void HandleProperty(AlignmentProperty property, ExcelReportCell cell)
        {
            cell.HorizontalAlignment = property.Alignment;
        }
    }
}
