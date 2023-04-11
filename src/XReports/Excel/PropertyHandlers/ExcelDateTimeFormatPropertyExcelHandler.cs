using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="ExcelDateTimeFormatProperty"/> during conversion to Excel.
    /// </summary>
    public class ExcelDateTimeFormatPropertyExcelHandler : PropertyHandler<ExcelDateTimeFormatProperty, ExcelReportCell>
    {
        private readonly DateTimeFormatPropertyExcelHandler standardHandler = new DateTimeFormatPropertyExcelHandler();

        /// <inheritdoc />
        public override int Priority => -1;

        /// <inheritdoc />
        protected override void HandleProperty(ExcelDateTimeFormatProperty property, ExcelReportCell cell)
        {
            this.standardHandler.Handle(property, cell);

            cell.NumberFormat = property.ExcelFormat;
        }
    }
}
