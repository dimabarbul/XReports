﻿using System;
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

            VerticalReportBuilder<(decimal, decimal, decimal, decimal, decimal)> builder = new VerticalReportBuilder<(decimal, decimal, decimal, decimal, decimal)>();
            builder.AddColumn("#", i => DateTime.Now)
                .AddProperty(new DateTimeFormatProperty("nnnn dd MMM yyyy"));
            builder.AddColumn("#2", i => i.Item2);
            builder.AddColumn("#3", i => i.Item3)
                .AddProperty(new DecimalFormatProperty(2));
            builder.AddColumn("#4", i => i.Item4.ToString());
            builder.AddColumn("#5", i => "Looooooooooong")
                .AddProperty(new MaxLengthProperty(5));

            IReportTable<ReportCell> reportTable = builder.Build(Enumerable.Range(1, 10000)
                .Select<int, (decimal, decimal, decimal, decimal, decimal)>(x => (x, x + 1, x + 2, x + 3, x + 4)));

            ReportConverter<ExcelReportCell> converter = new ReportConverter<ExcelReportCell>(
                new IPropertyHandler<ExcelReportCell>[]
                {
                    new ExcelAlignmentPropertyHandler(),
                    new ExcelBoldPropertyHandler(),
                    new ExcelDateTimeFormatPropertyHandler(),
                    new ExcelDecimalFormatPropertyHandler(),
                    new ExcelMaxLengthPropertyHandler(),
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
