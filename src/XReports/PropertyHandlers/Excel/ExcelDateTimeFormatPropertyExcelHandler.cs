using XReports.Models;
using XReports.Properties.Excel;

namespace XReports.PropertyHandlers.Excel
{
    public class ExcelDateTimeFormatPropertyExcelHandler : PropertyHandler<ExcelDateTimeFormatProperty, ExcelReportCell>
    {
        private readonly DateTimeFormatPropertyExcelHandler standardHandler = new DateTimeFormatPropertyExcelHandler();

        public override int Priority => -1;

        protected override void HandleProperty(ExcelDateTimeFormatProperty property, ExcelReportCell cell)
        {
            this.standardHandler.Handle(property, cell);

            cell.NumberFormat = property.ExcelFormat;
        }
    }
}
