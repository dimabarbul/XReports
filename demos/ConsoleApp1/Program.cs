using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Reports.Core;
using Reports.Core.Enums;
using Reports.Core.Extensions;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Core.PropertyHandlers;
using Reports.Core.SchemaBuilders;
using Reports.Excel.EpplusWriter;
using Reports.Extensions.Properties;
using Reports.Extensions.Properties.PropertyHandlers.Excel;
using Reports.Extensions.Properties.PropertyHandlers.Html;
using Reports.Html.StringWriter;
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
                .AddProperties(new PercentFormatProperty(1){PostfixText = null}, columnSameFormatProperty);
            builder.AddColumn("Colored", i => i.Item1 % 10)
                .AddProperties(new ColorProperty(Color.Yellow, Color.Black), columnSameFormatProperty);
            builder.AddColumn("With custom format", i => i.Item2 * 100)
                .AddProperties(new MyCustomFormatProperty(), columnSameFormatProperty, new BorderProperty(Color.Chocolate));

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
            ReportConverter<MyExcelReportCell> converter = new ReportConverter<MyExcelReportCell>(
                new IPropertyHandler<MyExcelReportCell>[]
                {
                    new AlignmentPropertyExcelHandler(),
                    new BoldPropertyExcelHandler(),
                    new ColorPropertyExcelHandler(),
                    new DateTimeFormatPropertyExcelHandler(),
                    new DecimalFormatPropertyExcelHandler(),
                    new MaxLengthPropertyExcelHandler(),
                    new PercentFormatPropertyExcelHandler(),

                    new BorderPropertyExcelHandler(),
                }
            );
            IReportTable<MyExcelReportCell> excelReportTable = converter.Convert(reportTable);

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
                    new AlignmentPropertyHtmlHandler(),
                    new BoldPropertyHtmlHandler(),
                    new ColorPropertyHtmlHandler(),
                    new DateTimeFormatPropertyHtmlHandler(),
                    new DecimalFormatPropertyHtmlHandler(),
                    new MaxLengthPropertyHtmlHandler(),
                    new PercentFormatPropertyHtmlHandler(),

                    new StandardHtmlMyCustomFormatPropertyHandler(),
                }
            );
            IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(reportTable);

            const string fileName = "/tmp/report.html";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            StringCellWriter cellWriter = new StringCellWriter();
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

    public class StandardHtmlMyCustomFormatPropertyHandler : PropertyHandler<MyCustomFormatProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(MyCustomFormatProperty property, HtmlReportCell cell)
        {
            decimal value = cell.GetValue<decimal>();
            string format = value >= 90 ? "F0" : "F1";

            cell.Html = value.ToString(format);
        }
    }

    public class ExcelMyCustomFormatPropertyHandler : PropertyHandler<MyCustomFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(MyCustomFormatProperty property, ExcelReportCell cell)
        {
            cell.NumberFormat = "[>=90]0;[<90]0.0";
        }
    }

    public class MyExcelReportCell : ExcelReportCell
    {
        public Color? BorderColor { get; set; }
    }

    public class BorderProperty : ReportCellProperty
    {
        public Color BorderColor { get; }

        public BorderProperty(Color borderColor)
        {
            this.BorderColor = borderColor;
        }
    }

    public class BorderPropertyExcelHandler : PropertyHandler<BorderProperty, MyExcelReportCell>
    {
        protected override void HandleProperty(BorderProperty property, MyExcelReportCell cell)
        {
            cell.BorderColor = property.BorderColor;
        }
    }
}
