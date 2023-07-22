using XReports.Converter;
using XReports.Excel;
using XReports.Excel.PropertyHandlers;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.AttributeHandlers;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

namespace XReports.Tests.Excel.Writers.EpplusWriterTests
{
    internal static class Helper
    {
        public static IReportTable<ExcelReportCell> CreateExcelReport()
        {
            IReportTable<ReportCell> reportTable = new AttributeBasedBuilder(new[] { new CommonAttributeHandler() })
                .BuildSchema<Model>()
                .BuildReportTable(new[]
                {
                    new Model()
                    {
                        Name = "John Doe",
                        Email = "johndoe@example.com",
                    },
                    new Model()
                    {
                        Name = "Jane Doe",
                        Email = "janedoe@example.com",
                    },
                });
            IReportTable<ExcelReportCell> excelReport = new ReportConverter<ExcelReportCell>(new[]
                {
                    new BoldPropertyExcelHandler(),
                })
                .Convert(reportTable);
            return excelReport;
        }

        public static string[] GetFlattenedReportValues()
        {
            return new[]
            {
                "Name", "Email",
                "John Doe", "johndoe@example.com",
                "Jane Doe", "janedoe@example.com",
            };
        }

        private class Model
        {
            [ReportColumn(1)]
            public string Name { get; set; }

            [ReportColumn(2)]
            [Bold]
            public string Email { get; set; }
        }
    }
}
