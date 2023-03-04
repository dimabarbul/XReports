using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using XReports.Core.Tests.Assertions;
using XReports.Core.Tests.Extensions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests
{
    public partial class ReportConverterTest
    {
        private readonly IEnumerable<int> singleItemSource = Enumerable.Range(1, 1);

        [Fact]
        public void ConvertShouldConvertAllCellsForVertical()
        {
            VerticalReportSchemaBuilder<int> builder = new VerticalReportSchemaBuilder<int>();
            builder.AddColumn("Value", i => i).AddProperties(new MyProperty());
            builder.AddColumn("Square", i => i * i).AddHeaderProperties(new MyProperty());
            builder.AddComplexHeader(0, "Header", 0, 1);
            builder.AddComplexHeaderProperties(new MyAnotherProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(new[] { 1, 2 });

            IReportTable<NewReportCell> converted = new ReportConverter<NewReportCell>().Convert(reportTable);

            converted.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell("Header", columnSpan: 2, properties: new MyAnotherProperty()),
                    null,
                },
                new[]
                {
                    this.CreateCell("Value"),
                    this.CreateCell("Square", properties: new MyProperty()),
                },
            });
            converted.Rows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell(1, properties: new MyProperty()),
                    this.CreateCell(1),
                },
                new[]
                {
                    this.CreateCell(2, properties: new MyProperty()),
                    this.CreateCell(4),
                },
            });
        }

        [Fact]
        public void ConvertShouldConvertAllCellsForHorizontal()
        {
            HorizontalReportSchemaBuilder<int> builder = new HorizontalReportSchemaBuilder<int>();
            builder.AddHeaderRow("#", i => i.ToString(CultureInfo.InvariantCulture))
                .AddProperties(new MyAnotherProperty());
            builder.AddRow("Value", i => i).AddProperties(new MyProperty());
            builder.AddRow("Square", i => i * i).AddHeaderProperties(new MyProperty());
            builder.AddComplexHeader(0, "Header", 0, 1);
            builder.AddComplexHeaderProperties(new MyAnotherProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(new[] { 1, 2 });

            IReportTable<NewReportCell> converted = new ReportConverter<NewReportCell>().Convert(reportTable);

            converted.HeaderRows.Clone().Should().BeEquivalentTo(new object[]
            {
                new[]
                {
                    this.CreateCell("#", columnSpan: 2),
                    null,
                    this.CreateCell("1", properties: new MyAnotherProperty()),
                    this.CreateCell("2", properties: new MyAnotherProperty()),
                },
            });
            converted.Rows.Clone().Should().BeEquivalentTo(new object[]
            {
                new[]
                {
                    this.CreateCell("Header", rowSpan: 2, properties: new MyAnotherProperty()),
                    this.CreateCell("Value"),
                    this.CreateCell(1, properties: new MyProperty()),
                    this.CreateCell(2, properties: new MyProperty()),
                },
                new[]
                {
                    null,
                    this.CreateCell("Square", properties: new MyProperty()),
                    this.CreateCell(1),
                    this.CreateCell(4),
                },
            });
        }

        [Fact]
        public void ConvertShouldNotEnumerateDataSourceWhenResultIsNotEnumeratedForVertical()
        {
            IEnumerable<int> GetData(Action onEnumerate)
            {
                yield return 1;
                onEnumerate();
            }

            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>();
            VerticalReportSchemaBuilder<int> builder = new VerticalReportSchemaBuilder<int>();
            builder.AddColumn("Value", i => i);
            bool enumerated = false;
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(GetData(() => enumerated = true));

            _ = reportConverter.Convert(reportTable);

            enumerated.Should().BeFalse();
        }

        [Fact]
        public void ConvertShouldNotEnumerateDataSourceWhenResultIsNotEnumeratedForHorizontal()
        {
            IEnumerable<int> GetData(Action onEnumerate)
            {
                yield return 1;
                onEnumerate();
            }

            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>();
            HorizontalReportSchemaBuilder<int> builder = new HorizontalReportSchemaBuilder<int>();
            builder.AddRow("Value", i => i);
            bool enumerated = false;
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(GetData(() => enumerated = true));

            _ = reportConverter.Convert(reportTable);

            enumerated.Should().BeFalse();
        }

        [Fact]
        public void ConvertShouldCallHandlersForHeaderPropertiesForVertical()
        {
            MyHandler firstHandler = new MyHandler("1", 0, false);
            MyHandler secondHandler = new MyHandler("2", 1, false);
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { secondHandler, firstHandler });
            VerticalReportSchemaBuilder<int> builder = new VerticalReportSchemaBuilder<int>();
            builder.AddColumn("Value", i => i)
                .AddHeaderProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);

            converted.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell(
                        "Value",
                        data: new[] { "1", "2" },
                        properties: new MyProperty()),
                },
            });
            converted.Rows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell(1),
                },
            });
        }

        [Fact]
        public void ConvertShouldCallHandlersForHeaderPropertiesForHorizontal()
        {
            MyHandler firstHandler = new MyHandler("1", 0, false);
            MyHandler secondHandler = new MyHandler("2", 1, false);
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { secondHandler, firstHandler });
            HorizontalReportSchemaBuilder<int> builder = new HorizontalReportSchemaBuilder<int>();
            builder.AddRow("Value", i => i)
                .AddHeaderProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);

            converted.HeaderRows.Clone().Should().BeEmpty();
            converted.Rows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell(
                        "Value",
                        data: new[] { "1", "2" },
                        properties: new MyProperty()),
                    this.CreateCell(1),
                },
            });
        }

        [Fact]
        public void ConvertShouldNotCallHandlersWhenReportIsNotIteratedForVertical()
        {
            List<MyHandler> breadcrumbs = new List<MyHandler>();
            MyHandler myHandler = new MyHandler(0, false, (handler, property) => breadcrumbs.Add(handler));
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { myHandler });
            VerticalReportSchemaBuilder<int> builder = new VerticalReportSchemaBuilder<int>();
            builder.AddColumn("Value", i => i)
                .AddProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            _ = reportConverter.Convert(reportTable);

            breadcrumbs.Should().BeEmpty();
        }

        [Fact]
        public void ConvertShouldNotCallHandlersWhenReportIsNotIteratedForHorizontal()
        {
            List<MyHandler> breadcrumbs = new List<MyHandler>();
            MyHandler myHandler = new MyHandler(0, false, (handler, property) => breadcrumbs.Add(handler));
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { myHandler });
            HorizontalReportSchemaBuilder<int> builder = new HorizontalReportSchemaBuilder<int>();
            builder.AddRow("Value", i => i)
                .AddProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            _ = reportConverter.Convert(reportTable);

            breadcrumbs.Should().BeEmpty();
        }

        [Fact]
        public void ConvertShouldCallHandlersInOrderForVertical()
        {
            MyHandler firstHandler = new MyHandler("1", 0, false);
            MyHandler secondHandler = new MyHandler("2", 1, false);
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { secondHandler, firstHandler });
            VerticalReportSchemaBuilder<int> builder = new VerticalReportSchemaBuilder<int>();
            builder.AddColumn("Value", i => i)
                .AddProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);

            converted.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell("Value"),
                },
            });
            converted.Rows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell(
                        1,
                        data: new[] { "1", "2" },
                        properties: new MyProperty()),
                },
            });
        }

        [Fact]
        public void ConvertShouldCallHandlersInOrderForHorizontal()
        {
            MyHandler firstHandler = new MyHandler("1", 0, false);
            MyHandler secondHandler = new MyHandler("2", 1, false);
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { secondHandler, firstHandler });
            HorizontalReportSchemaBuilder<int> builder = new HorizontalReportSchemaBuilder<int>();
            builder.AddRow("Value", i => i)
                .AddProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);

            converted.HeaderRows.Clone().Should().BeEmpty();
            converted.Rows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell("Value"),
                    this.CreateCell(
                        1,
                        data: new[] { "1", "2" },
                        properties: new MyProperty()),
                },
            });
        }

        [Fact]
        public void ConvertShouldCallHandlersForEachPropertyForEachCellForVertical()
        {
            List<ReportCellProperty> breadcrumbs = new List<ReportCellProperty>();
            MyHandler myHandler = new MyHandler(0, false, (handler, property) => breadcrumbs.Add(property));
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { myHandler });
            int dataCount = 2;
            VerticalReportSchemaBuilder<int> builder = new VerticalReportSchemaBuilder<int>();
            builder.AddColumn("Value", i => i)
                .AddProperties(new MyProperty(), new MyAnotherProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(Enumerable.Range(1, dataCount));

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);
            converted.Enumerate();

            breadcrumbs.Should().BeEquivalentTo(
                Enumerable.Range(0, dataCount)
                    .SelectMany(_ => new ReportCellProperty[] { new MyProperty(), new MyAnotherProperty() }));
        }

        [Fact]
        public void ConvertShouldCallHandlersForEachPropertyForEachCellForHorizontal()
        {
            List<ReportCellProperty> breadcrumbs = new List<ReportCellProperty>();
            MyHandler myHandler = new MyHandler(0, false, (handler, property) => breadcrumbs.Add(property));
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { myHandler });
            int dataCount = 2;
            HorizontalReportSchemaBuilder<int> builder = new HorizontalReportSchemaBuilder<int>();
            builder.AddRow("Value", i => i)
                .AddProperties(new MyProperty(), new MyAnotherProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(Enumerable.Range(1, dataCount));

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);
            converted.Enumerate();

            breadcrumbs.Should().BeEquivalentTo(
                Enumerable.Range(0, dataCount)
                    .SelectMany(_ => new ReportCellProperty[] { new MyProperty(), new MyAnotherProperty() }));
        }

        [Fact]
        public void ConvertShouldNotCallNextHandlersWhenPropertyHasBeenProcessedForVertical()
        {
            MyHandler firstHandler = new MyHandler("1", 0, true);
            MyHandler secondHandler = new MyHandler("2", 1, true);
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { secondHandler, firstHandler });
            VerticalReportSchemaBuilder<int> builder = new VerticalReportSchemaBuilder<int>();
            builder.AddColumn("Value", i => i)
                .AddProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);

            converted.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell("Value"),
                },
            });
            converted.Rows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell(
                        1,
                        data: new[] { "1" }),
                },
            });
        }

        [Fact]
        public void ConvertShouldNotCallNextHandlersWhenPropertyHasBeenProcessedForHorizontal()
        {
            MyHandler firstHandler = new MyHandler("1", 0, true);
            MyHandler secondHandler = new MyHandler("2", 1, true);
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { secondHandler, firstHandler });
            HorizontalReportSchemaBuilder<int> builder = new HorizontalReportSchemaBuilder<int>();
            builder.AddRow("Value", i => i)
                .AddProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);

            converted.HeaderRows.Clone().Should().BeEmpty();
            converted.Rows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell("Value"),
                    this.CreateCell(
                        1,
                        data: new[] { "1" }),
                },
            });
        }

        [Fact]
        public void ConvertShouldLeavePropertyWhenPropertyIsNotProcessedForVertical()
        {
            MyHandler myHandler = new MyHandler(0, false);
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { myHandler });
            VerticalReportSchemaBuilder<int> builder = new VerticalReportSchemaBuilder<int>();
            builder.AddColumn("Value", i => i)
                .AddProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);

            converted.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell("Value"),
                },
            });
            converted.Rows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell(1, properties: new MyProperty()),
                },
            });
        }

        [Fact]
        public void ConvertShouldLeavePropertyWhenPropertyIsNotProcessedForHorizontal()
        {
            MyHandler myHandler = new MyHandler(0, false);
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { myHandler });
            HorizontalReportSchemaBuilder<int> builder = new HorizontalReportSchemaBuilder<int>();
            builder.AddRow("Value", i => i)
                .AddProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);

            converted.HeaderRows.Clone().Should().BeEmpty();
            converted.Rows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell("Value"),
                    this.CreateCell(1, properties: new MyProperty()),
                },
            });
        }

        [Fact]
        public void ConvertShouldDeletePropertyWhenPropertyIsProcessedForVertical()
        {
            MyHandler myHandler = new MyHandler(0, true);
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { myHandler });
            VerticalReportSchemaBuilder<int> builder = new VerticalReportSchemaBuilder<int>();
            builder.AddColumn("Value", i => i)
                .AddProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);

            converted.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell("Value"),
                },
            });
            converted.Rows.Should().Equal(new[]
            {
                new[]
                {
                    this.CreateCell(1),
                },
            });
        }

        [Fact]
        public void ConvertShouldDeletePropertyWhenPropertyIsProcessedForHorizontal()
        {
            MyHandler myHandler = new MyHandler(0, true);
            ReportConverter<NewReportCell> reportConverter = new ReportConverter<NewReportCell>(
                new[] { myHandler });
            HorizontalReportSchemaBuilder<int> builder = new HorizontalReportSchemaBuilder<int>();
            builder.AddRow("Value", i => i)
                .AddProperties(new MyProperty());
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(this.singleItemSource);

            IReportTable<NewReportCell> converted = reportConverter.Convert(reportTable);

            converted.HeaderRows.Clone().Should().BeEmpty();
            converted.Rows.Clone().Should().BeEquivalentTo(new object[]
            {
                new[]
                {
                    this.CreateCell("Value"),
                    this.CreateCell(1),
                },
            });
        }
    }
}
