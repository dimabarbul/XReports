using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Reports;
using Reports.Builders;
using Reports.Excel.Models;
using Reports.Excel.Writers;
using Reports.Extensions;
using Reports.Extensions.Properties;
using Reports.Extensions.Properties.Handlers.Excel;
using Reports.Interfaces;
using Reports.Models;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // EntityAttributeBuilderHelper helper = new EntityAttributeBuilderHelper();
            // VerticalReportBuilder<MyEntity> builder = new VerticalReportBuilder<MyEntity>();
            //
            // helper.BuildVerticalReport(builder);
            //
            // IReportTable reportTable = builder.Build(new[]
            // {
            //     new MyEntity()
            //     {
            //         Id = 1,
            //         Name = "John",
            //     },
            //     new MyEntity()
            //     {
            //         Id = 2,
            //         Name = "Jane",
            //     },
            // });

            VerticalReportBuilder<(int, decimal)> builder = new VerticalReportBuilder<(int, decimal)>();
            builder.AddColumn("Now", i => DateTime.Now)
                .AddProperty(new DateTimeFormatProperty("nnnn dd MMM yyyy"));
            builder.AddColumn("Now", i => DateTime.Now)
                .AddProperty(new DateTimeFormatProperty("dd/MM/yyyy HH:mm:ss"));
            builder.AddColumn("Integer", i => i.Item1);
            builder.AddColumn("Without formatting", i => i.Item2);
            builder.AddColumn("With 2 decimals", i => i.Item2)
                .AddProperty(new DecimalFormatProperty(2));
            builder.AddColumn("String", i => i.Item2.ToString());
            builder.AddColumn("With max.length", i => "Looooooooooong")
                .AddProperty(new MaxLengthProperty(5));
            builder.AddColumn("Percent", i => i.Item2)
                .AddProperty(new PercentFormatProperty(1));

            Random random = new Random(DateTime.Now.Millisecond);
            IReportTable<ReportCell> reportTable = builder.Build(Enumerable.Range(1, 10000)
                .Select(x => (random.Next(), (decimal) random.NextDouble())));

            ReportConverter<ExcelReportCell> converter = new ReportConverter<ExcelReportCell>(
                new IPropertyHandler<ExcelReportCell>[]
                {
                    new ExcelAlignmentPropertyHandler(),
                    new ExcelBoldPropertyHandler(),
                    new ExcelDateTimeFormatPropertyHandler(),
                    new ExcelDecimalFormatPropertyHandler(),
                    new ExcelMaxLengthPropertyHandler(),
                    new ExcelPercentFormatPropertyHandler(),
                }
            );
            IReportTable<ExcelReportCell> excelReportTable = converter.Convert(reportTable);

            const string fileName = "/tmp/1.xlsx";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            ExcelWriter writer = new ExcelWriter();
            Stopwatch sw = Stopwatch.StartNew();
            writer.WriteToFile(excelReportTable, fileName);
            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");

            return;
        }

        // public class MyEntity
        // {
        //     [ReportVariable(0, "ID")]
        //     public int Id { get; set; }
        //
        //     [ReportVariable(1, "Name")]
        //     public string Name { get; set; }
        // }
    }
}
