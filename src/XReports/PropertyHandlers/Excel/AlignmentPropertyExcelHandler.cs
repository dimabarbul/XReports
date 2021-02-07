using System;
using XReports.Enums;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class AlignmentPropertyExcelHandler : PropertyHandler<AlignmentProperty, ExcelReportCell>
    {
        protected override void HandleProperty(AlignmentProperty property, ExcelReportCell cell)
        {
            cell.HorizontalAlignment = this.GetHorizontalAlignment(property.Alignment);
        }

        private Alignment GetHorizontalAlignment(Alignment alignment)
        {
            return alignment switch
            {
                Alignment.Center => Alignment.Center,
                Alignment.Left => Alignment.Left,
                Alignment.Right => Alignment.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(alignment)),
            };
        }
    }
}
