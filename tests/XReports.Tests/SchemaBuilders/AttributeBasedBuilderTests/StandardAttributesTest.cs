using System;
using System.Drawing;
using System.Linq;
using XReports.AttributeHandlers;
using XReports.DependencyInjection;
using XReports.Enums;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;
using XReports.Properties.Excel;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class StandardAttributesTest
    {
        [Fact]
        public void BuildSchemaShouldProcessStandardAttributes()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(
                new TypesCollection<IAttributeHandler>()
                    .AddFromAssembly(typeof(CommonAttributeHandler).Assembly)
                    .Select(t => (IAttributeHandler)Activator.CreateInstance(t)));

            IReportSchema<Data> schema = builder.BuildSchema<Data>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[] { new Data() });
            reportTable.HeaderRows.Clone().Should().BeEquivalentTo(new[]
            {
                new[]
                {
                    this.CreateReportCell("Alignment", new AlignmentProperty(Alignment.Left)),
                    this.CreateReportCell("Bold", new BoldProperty()),
#if NET6_0_OR_GREATER
                    this.CreateReportCell("FontColorFromKnownColor", new ColorProperty(Color.Green)),
                    this.CreateReportCell(
                        "FontAndBackgroundColorsFromKnownColor",
                        new ColorProperty(Color.Green, Color.Black)),
#endif
                    this.CreateReportCell("FontColorFromColorName", new ColorProperty(Color.Green)),
                    this.CreateReportCell(
                        "FontAndBackgroundColorsFromColorName",
                        new ColorProperty(Color.Green, Color.Black)),
                    this.CreateReportCell("FontColorFromColorNumber", new ColorProperty(Color.FromArgb(0, 255, 0))),
                    this.CreateReportCell(
                        "FontAndBackgroundColorsFromColorNumber",
                        new ColorProperty(Color.FromArgb(0, 255, 0), Color.FromArgb(0, 0, 0))),
                    this.CreateReportCell("Custom", new MyProperty()),
                    this.CreateReportCell("DateTimeFormat"),
                    this.CreateReportCell("ExcelDateTimeFormat"),
                    this.CreateReportCell("DecimalPrecision"),
                    this.CreateReportCell("DecimalPrecisionWithoutTrailingZeros"),
                    this.CreateReportCell("MaxLength", new MaxLengthProperty(20)),
                    this.CreateReportCell("MaxLengthWithCustomText", new MaxLengthProperty(20, "...")),
                    this.CreateReportCell("PercentFormat"),
                    this.CreateReportCell("PercentFormatWithoutTrailingZeros"),
                    this.CreateReportCell("PercentFormatWithCustomText"),
                    this.CreateReportCell("PercentFormatWithoutTrailingZerosWithCustomText"),
                    this.CreateReportCell("SameColumnFormat"),
                },
            });
            reportTable.Rows.Clone().Should().BeEquivalentTo(new[]
            {
                new[]
                {
                    this.CreateReportCell(default(string), new AlignmentProperty(Alignment.Left)),
                    this.CreateReportCell(default(string), new BoldProperty()),
#if NET6_0_OR_GREATER
                    this.CreateReportCell(default(string), new ColorProperty(Color.Red)),
                    this.CreateReportCell(default(string), new ColorProperty(Color.Red, Color.Blue)),
#endif
                    this.CreateReportCell(default(string), new ColorProperty(Color.Red)),
                    this.CreateReportCell(default(string), new ColorProperty(Color.Red, Color.Blue)),
                    this.CreateReportCell(default(string), new ColorProperty(Color.FromArgb(255, 0, 0))),
                    this.CreateReportCell(default(string), new ColorProperty(Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 255))),
                    this.CreateReportCell(default(string), new MyProperty()),
                    this.CreateReportCell(default(DateTime), new DateTimeFormatProperty("O")),
                    this.CreateReportCell(
                        default(DateTime),
                        new ExcelDateTimeFormatProperty("HH:mm:ss tt", "HH:MM:SS AM/PM")),
                    this.CreateReportCell(default(decimal), new DecimalPrecisionProperty(1)),
                    this.CreateReportCell(
                        default(decimal),
                        new DecimalPrecisionProperty(1, false)),
                    this.CreateReportCell(default(string), new MaxLengthProperty(10)),
                    this.CreateReportCell(default(string), new MaxLengthProperty(10, "...")),
                    this.CreateReportCell(default(decimal), new PercentFormatProperty(1)),
                    this.CreateReportCell(
                        default(decimal),
                        new PercentFormatProperty(1, preserveTrailingZeros: false)),
                    this.CreateReportCell(default(decimal), new PercentFormatProperty(1, " (%)")),
                    this.CreateReportCell(
                        default(decimal),
                        new PercentFormatProperty(1, " (%)", false)),
                    this.CreateReportCell(default(string), new SameColumnFormatProperty()),
                },
            });
        }

        private ReportCell CreateReportCell<T>(T value, ReportCellProperty property = null)
        {
            ReportCell cell = new ReportCell();
            cell.SetValue(value);

            if (property != null)
            {
                cell.AddProperty(property);
            }

            return cell;
        }
    }
}
