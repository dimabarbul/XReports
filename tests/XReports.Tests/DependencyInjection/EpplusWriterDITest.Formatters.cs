using OfficeOpenXml;
using XReports.Excel;
using XReports.Excel.Writers;
using XReports.ReportCellProperties;

namespace XReports.Tests.DependencyInjection
{
    public partial class EpplusWriterDITest
    {
        private class Formatter : IEpplusFormatter
        {
            public void Format(ExcelRange worksheetCell, ExcelReportCell cell)
            {
                throw new System.NotImplementedException();
            }
        }

        private class BoldFormatter : EpplusFormatter<BoldProperty>
        {
            protected override void Format(ExcelRange worksheetCell, ExcelReportCell cell, BoldProperty property)
            {
                throw new System.NotImplementedException();
            }
        }

        private abstract class AbstractFormatter : IEpplusFormatter
        {
            public abstract void Format(ExcelRange worksheetCell, ExcelReportCell cell);
        }
    }
}
