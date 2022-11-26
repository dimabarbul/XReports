using OfficeOpenXml;
using XReports.EpplusFormatters;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;

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
