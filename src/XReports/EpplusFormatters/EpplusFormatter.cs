using OfficeOpenXml;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.EpplusFormatters
{
    public abstract class EpplusFormatter<TProperty> : IEpplusFormatter
        where TProperty : ReportCellProperty
    {
        public void Format(ExcelRange worksheetCell, ExcelReportCell cell)
        {
            TProperty property = cell.GetProperty<TProperty>();
            if (property == null)
            {
                return;
            }

            this.Format(worksheetCell, cell, property);
        }

        protected abstract void Format(ExcelRange worksheetCell, ExcelReportCell cell, TProperty property);
    }
}
