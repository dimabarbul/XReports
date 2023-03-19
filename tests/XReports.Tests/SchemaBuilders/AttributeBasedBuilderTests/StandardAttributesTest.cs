using System;
using System.Drawing;
using System.Linq;
using XReports.DependencyInjection;
using XReports.ReportCellProperties;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.AttributeHandlers;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
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
            reportTable.HeaderRows.Clone().Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Alignment", new AlignmentProperty(Alignment.Left)),
                    ReportCellHelper.CreateReportCell("Bold", new BoldProperty()),
#if NET6_0_OR_GREATER
                    ReportCellHelper.CreateReportCell("FontColorFromKnownColor", new ColorProperty(Color.Green)),
                    ReportCellHelper.CreateReportCell(
                        "FontAndBackgroundColorsFromKnownColor",
                        new ColorProperty(Color.Green, Color.Black)),
#endif
                    ReportCellHelper.CreateReportCell("FontColorFromColorName", new ColorProperty(Color.Green)),
                    ReportCellHelper.CreateReportCell(
                        "FontAndBackgroundColorsFromColorName",
                        new ColorProperty(Color.Green, Color.Black)),
                    ReportCellHelper.CreateReportCell("FontColorFromColorNumber", new ColorProperty(Color.FromArgb(0, 255, 0))),
                    ReportCellHelper.CreateReportCell(
                        "FontAndBackgroundColorsFromColorNumber",
                        new ColorProperty(Color.FromArgb(0, 255, 0), Color.FromArgb(0, 0, 0))),
                    ReportCellHelper.CreateReportCell("Custom", new MyProperty()),
                    ReportCellHelper.CreateReportCell("DateTimeFormat"),
                    ReportCellHelper.CreateReportCell("ExcelDateTimeFormat"),
                    ReportCellHelper.CreateReportCell("DecimalPrecision"),
                    ReportCellHelper.CreateReportCell("DecimalPrecisionWithoutTrailingZeros"),
                    ReportCellHelper.CreateReportCell("MaxLength", new MaxLengthProperty(20)),
                    ReportCellHelper.CreateReportCell("MaxLengthWithCustomText", new MaxLengthProperty(20, "...")),
                    ReportCellHelper.CreateReportCell("PercentFormat"),
                    ReportCellHelper.CreateReportCell("PercentFormatWithoutTrailingZeros"),
                    ReportCellHelper.CreateReportCell("PercentFormatWithCustomText"),
                    ReportCellHelper.CreateReportCell("PercentFormatWithoutTrailingZerosWithCustomText"),
                    ReportCellHelper.CreateReportCell("SameColumnFormat"),
                },
            });
            reportTable.Rows.Clone().Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(default(string), new AlignmentProperty(Alignment.Left)),
                    ReportCellHelper.CreateReportCell(default(string), new BoldProperty()),
#if NET6_0_OR_GREATER
                    ReportCellHelper.CreateReportCell(default(string), new ColorProperty(Color.Red)),
                    ReportCellHelper.CreateReportCell(default(string), new ColorProperty(Color.Red, Color.Blue)),
#endif
                    ReportCellHelper.CreateReportCell(default(string), new ColorProperty(Color.Red)),
                    ReportCellHelper.CreateReportCell(default(string), new ColorProperty(Color.Red, Color.Blue)),
                    ReportCellHelper.CreateReportCell(default(string), new ColorProperty(Color.FromArgb(255, 0, 0))),
                    ReportCellHelper.CreateReportCell(default(string), new ColorProperty(Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 255))),
                    ReportCellHelper.CreateReportCell(default(string), new MyProperty()),
                    ReportCellHelper.CreateReportCell(default(DateTime), new DateTimeFormatProperty("O")),
                    ReportCellHelper.CreateReportCell(
                        default(DateTime),
                        new ExcelDateTimeFormatProperty("HH:mm:ss tt", "HH:MM:SS AM/PM")),
                    ReportCellHelper.CreateReportCell(default(decimal), new DecimalPrecisionProperty(1)),
                    ReportCellHelper.CreateReportCell(
                        default(decimal),
                        new DecimalPrecisionProperty(1, false)),
                    ReportCellHelper.CreateReportCell(default(string), new MaxLengthProperty(10)),
                    ReportCellHelper.CreateReportCell(default(string), new MaxLengthProperty(10, "...")),
                    ReportCellHelper.CreateReportCell(default(decimal), new PercentFormatProperty(1)),
                    ReportCellHelper.CreateReportCell(
                        default(decimal),
                        new PercentFormatProperty(1, preserveTrailingZeros: false)),
                    ReportCellHelper.CreateReportCell(default(decimal), new PercentFormatProperty(1, " (%)")),
                    ReportCellHelper.CreateReportCell(
                        default(decimal),
                        new PercentFormatProperty(1, " (%)", false)),
                    ReportCellHelper.CreateReportCell(default(string), new SameColumnFormatProperty()),
                },
            });
        }
    }
}
