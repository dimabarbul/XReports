using System;
using Reports.Enums;
using Reports.Models;
using Reports.Properties;

namespace Reports.PropertyHandlers.Excel
{
    public class AlignmentPropertyExcelHandler : PropertyHandler<AlignmentProperty, ExcelReportCell>
    {
        protected override void HandleProperty(AlignmentProperty property, ExcelReportCell cell)
        {
            cell.AlignmentType = this.GetAlignment(property.AlignmentType);
        }

        private AlignmentType GetAlignment(AlignmentType alignmentType)
        {
            return alignmentType switch
            {
                AlignmentType.Center => AlignmentType.Center,
                AlignmentType.Left => AlignmentType.Left,
                AlignmentType.Right => AlignmentType.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(alignmentType)),
            };
        }
    }
}
