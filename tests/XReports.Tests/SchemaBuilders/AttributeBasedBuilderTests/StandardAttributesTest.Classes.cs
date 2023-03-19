using System;
using System.Drawing;
using XReports.SchemaBuilders.Attributes;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class StandardAttributesTest
    {
        private class Data
        {
            [Alignment(ReportCellProperties.Alignment.Left)]
            [Alignment(ReportCellProperties.Alignment.Left, IsHeader = true)]
            [ReportColumn(1, nameof(Alignment))]
            public string Alignment { get; set; }

            [Bold]
            [Bold(IsHeader = true)]
            [ReportColumn(2, nameof(Bold))]
            public string Bold { get; set; }

#if NET6_0_OR_GREATER
            [Color(KnownColor.Red)]
            [Color(KnownColor.Green, IsHeader = true)]
            [ReportColumn(3, nameof(FontColorFromKnownColor))]
            public string FontColorFromKnownColor { get; set; }

            [Color(KnownColor.Red, KnownColor.Blue)]
            [Color(KnownColor.Green, KnownColor.Black, IsHeader = true)]
            [ReportColumn(4, nameof(FontAndBackgroundColorsFromKnownColor))]
            public string FontAndBackgroundColorsFromKnownColor { get; set; }
#endif

            [Color("red")]
            [Color("green", IsHeader = true)]
            [ReportColumn(5, nameof(FontColorFromColorName))]
            public string FontColorFromColorName { get; set; }

            [Color("red", "blue")]
            [Color("green", "black", IsHeader = true)]
            [ReportColumn(6, nameof(FontAndBackgroundColorsFromColorName))]
            public string FontAndBackgroundColorsFromColorName { get; set; }

            [Color(0xFF0000)]
            [Color(0x00FF00, IsHeader = true)]
            [ReportColumn(7, nameof(FontColorFromColorNumber))]
            public string FontColorFromColorNumber { get; set; }

            [Color(0xFF0000, 0x0000FF)]
            [Color(0x00FF00, 0, IsHeader = true)]
            [ReportColumn(8, nameof(FontAndBackgroundColorsFromColorNumber))]
            public string FontAndBackgroundColorsFromColorNumber { get; set; }

            [CustomProperty(typeof(MyProperty))]
            [CustomProperty(typeof(MyProperty), IsHeader = true)]
            [ReportColumn(9, nameof(Custom))]
            public string Custom { get; set; }

            [DateTimeFormat("O")]
            [ReportColumn(10, nameof(DateTimeFormat))]
            public DateTime DateTimeFormat { get; set; }

            [ExcelDateTimeFormat("HH:mm:ss tt", "HH:MM:SS AM/PM")]
            [ReportColumn(11, nameof(ExcelDateTimeFormat))]
            public DateTime ExcelDateTimeFormat { get; set; }

            [DecimalPrecision(1)]
            [ReportColumn(12, nameof(DecimalPrecision))]
            public decimal DecimalPrecision { get; set; }

            [DecimalPrecision(1, PreserveTrailingZeros = false)]
            [ReportColumn(13, nameof(DecimalPrecisionWithoutTrailingZeros))]
            public decimal DecimalPrecisionWithoutTrailingZeros { get; set; }

            [MaxLength(10)]
            [MaxLength(20, IsHeader = true)]
            [ReportColumn(14, nameof(MaxLength))]
            public string MaxLength { get; set; }

            [MaxLength(10, Text = "...")]
            [MaxLength(20, Text = "...", IsHeader = true)]
            [ReportColumn(15, nameof(MaxLengthWithCustomText))]
            public string MaxLengthWithCustomText { get; set; }

            [PercentFormat(1)]
            [ReportColumn(16, nameof(PercentFormat))]
            public decimal PercentFormat { get; set; }

            [PercentFormat(1, PreserveTrailingZeros = false)]
            [ReportColumn(17, nameof(PercentFormatWithoutTrailingZeros))]
            public decimal PercentFormatWithoutTrailingZeros { get; set; }

            [PercentFormat(1, PostfixText = " (%)")]
            [ReportColumn(18, nameof(PercentFormatWithCustomText))]
            public decimal PercentFormatWithCustomText { get; set; }

            [PercentFormat(1, PreserveTrailingZeros = false, PostfixText = " (%)")]
            [ReportColumn(19, nameof(PercentFormatWithoutTrailingZerosWithCustomText))]
            public decimal PercentFormatWithoutTrailingZerosWithCustomText { get; set; }

            [SameColumnFormat]
            [ReportColumn(20, nameof(SameColumnFormat))]
            public string SameColumnFormat { get; set; }
        }
    }
}
