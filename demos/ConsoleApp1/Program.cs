using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Reports;
using Reports.Excel.EpplusWriter;
using Reports.Excel.Models;
using Reports.Extensions;
using Reports.Extensions.Properties;
using Reports.Extensions.Properties.PropertyHandlers.Excel;
using Reports.Extensions.Properties.PropertyHandlers.StandardHtml;
using Reports.Html.Enums;
using Reports.Html.Models;
using Reports.Html.StringWriter;
using Reports.Interfaces;
using Reports.Models;
using Reports.PropertyHandlers;
using Reports.SchemaBuilders;
using StringWriter = Reports.Html.StringWriter.StringWriter;

namespace ConsoleApp1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            VerticalReportSchemaBuilder<(int, decimal)> builder = CreateBuilder();
            IReportTable<ReportCell> reportTable = BuildReportTable(builder);

            Console.Write("Export to excel? y=excel n=html: ");
            bool isExcel = Console.ReadLine()?.ToLower() == "y";

            if (isExcel)
            {
                ExportToExcel(reportTable);
            }
            else
            {
                await ExportToHtmlAsync(reportTable);
            }

            return;
        }

        private static VerticalReportSchemaBuilder<(int, decimal)> CreateBuilder()
        {
            ColumnSameFormatProperty columnSameFormatProperty = new ColumnSameFormatProperty();

            VerticalReportSchemaBuilder<(int, decimal)> builder = new VerticalReportSchemaBuilder<(int, decimal)>();
            builder.AddColumn("Now", i => DateTime.Now)
                .AddProperties(new DateTimeFormatProperty("dd MMM yyyy"), columnSameFormatProperty);
            builder.AddColumn("Now", i => DateTime.Now)
                .AddProperties(new DateTimeFormatProperty("dd/MM/yyyy HH:mm:ss"), columnSameFormatProperty);
            builder.AddColumn("Integer", i => i.Item1);
            builder.AddColumn("Without formatting", i => i.Item2);
            builder.AddColumn("With 2 decimals", i => i.Item2)
                .AddProperties(new DecimalFormatProperty(2), columnSameFormatProperty);
            builder.AddColumn("String", i => i.Item2.ToString());
            builder.AddColumn("Is odd", i => i.Item1 % 2 == 0
                ? "YES"
                : (string) null);
            builder.AddColumn("With max.length", i => "Some very loooooong text")
                .AddProperties(new MaxLengthProperty(15));
            builder.AddColumn("Percent", i => i.Item2)
                .AddProperties(new PercentFormatProperty(1), columnSameFormatProperty);
            builder.AddColumn("Colored", i => i.Item1 % 10)
                .AddProperties(new ColorProperty(Color.Yellow, Color.Black), columnSameFormatProperty);
            builder.AddColumn("With custom format", i => i.Item2 * 100)
                .AddProperties(new MyCustomFormatProperty(), columnSameFormatProperty);

            builder.AddComplexHeader(0, "Date", 0, 1);

            return builder;
        }

        private static IReportTable<ReportCell> BuildReportTable(VerticalReportSchemaBuilder<(int, decimal)> builder)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(Enumerable.Range(1, 1000)
                .Select(x => (random.Next(), (decimal) random.NextDouble())));
            return reportTable;
        }

        private static void ExportToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> converter = new ReportConverter<ExcelReportCell>(
                new IPropertyHandler<ExcelReportCell>[]
                {
                    new ExcelAlignmentPropertyHandler(),
                    new ExcelBoldPropertyHandler(),
                    new ExcelColorPropertyHandler(),
                    new ExcelDateTimeFormatPropertyHandler(),
                    new ExcelDecimalFormatPropertyHandler(),
                    new ExcelMaxLengthPropertyHandler(),
                    new ExcelPercentFormatPropertyHandler(),
                }
            );
            IReportTable<ExcelReportCell> excelReportTable = converter.Convert(reportTable);

            const string fileName = "/tmp/report.xlsx";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            EpplusWriter writer = new MyExcelWriter();
            Stopwatch sw = Stopwatch.StartNew();
            writer.WriteToFile(excelReportTable, fileName);
            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
        }

        private static async Task ExportToHtmlAsync(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(
                new IPropertyHandler<HtmlReportCell>[]
                {
                    new StandardHtmlAlignmentPropertyHandler(),
                    new StandardHtmlBoldPropertyHandler(),
                    new StandardHtmlColorPropertyHandler(),
                    new StandardHtmlDateTimeFormatPropertyHandler(),
                    new StandardHtmlDecimalFormatPropertyHandler(),
                    new StandardHtmlMaxLengthPropertyHandler(),
                    new StandardHtmlPercentFormatPropertyHandler(),

                    new StandardHtmlMyCustomFormatPropertyHandler(),
                }
            );
            IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(reportTable);

            const string fileName = "/tmp/report.html";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            GeneralCellWriter cellWriter = new GeneralCellWriter();
            StringWriter writer = new StringWriter(cellWriter.WriteHeaderCell, cellWriter.WriteBodyCell);
            Stopwatch sw = Stopwatch.StartNew();
            await writer.WriteToFileAsync(htmlReportTable, fileName);
            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
        }
    }

    public class MyCustomFormatProperty : ReportCellProperty
    {
    }

    public class StandardHtmlMyCustomFormatPropertyHandler : SingleTypePropertyHandler<MyCustomFormatProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(MyCustomFormatProperty property, HtmlReportCell cell)
        {
            decimal value = cell.GetValue<decimal>();
            string format = value >= 90 ? "F0" : "F1";

            cell.Html = value.ToString(format);
        }
    }

    public class ExcelMyCustomFormatPropertyHandler : SingleTypePropertyHandler<MyCustomFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(MyCustomFormatProperty property, ExcelReportCell cell)
        {
            cell.NumberFormat = "[>=90]0;[<90]0.0";
        }
    }
}
