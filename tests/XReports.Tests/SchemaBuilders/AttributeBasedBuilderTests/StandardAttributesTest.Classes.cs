using System;
using System.Drawing;
using XReports.Attributes;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class StandardAttributesTest
    {
        private class Data
        {
            [Alignment(Enums.Alignment.Left)]
            [Alignment(Enums.Alignment.Left, IsHeader = true)]
            [ReportVariable(1, nameof(Alignment))]
            public string Alignment { get; set; }

            [Bold]
            [Bold(IsHeader = true)]
            [ReportVariable(2, nameof(Bold))]
            public string Bold { get; set; }

#if NET6_0_OR_GREATER
            [Color(KnownColor.Red)]
            [Color(KnownColor.Green, IsHeader = true)]
            [ReportVariable(3, nameof(FontColorFromKnownColor))]
            public string FontColorFromKnownColor { get; set; }

            [Color(KnownColor.Red, KnownColor.Blue)]
            [Color(KnownColor.Green, KnownColor.Black, IsHeader = true)]
            [ReportVariable(4, nameof(FontAndBackgroundColorsFromKnownColor))]
            public string FontAndBackgroundColorsFromKnownColor { get; set; }
#endif

            [Color("red")]
            [Color("green", IsHeader = true)]
            [ReportVariable(5, nameof(FontColorFromColorName))]
            public string FontColorFromColorName { get; set; }

            [Color("red", "blue")]
            [Color("green", "black", IsHeader = true)]
            [ReportVariable(6, nameof(FontAndBackgroundColorsFromColorName))]
            public string FontAndBackgroundColorsFromColorName { get; set; }

            [Color(0xFF0000)]
            [Color(0x00FF00, IsHeader = true)]
            [ReportVariable(7, nameof(FontColorFromColorNumber))]
            public string FontColorFromColorNumber { get; set; }

            [Color(0xFF0000, 0x0000FF)]
            [Color(0x00FF00, 0, IsHeader = true)]
            [ReportVariable(8, nameof(FontAndBackgroundColorsFromColorNumber))]
            public string FontAndBackgroundColorsFromColorNumber { get; set; }

            [CustomProperty(typeof(MyProperty))]
            [CustomProperty(typeof(MyProperty), IsHeader = true)]
            [ReportVariable(9, nameof(Custom))]
            public string Custom { get; set; }

            [DateTimeFormat("O")]
            [ReportVariable(10, nameof(DateTimeFormat))]
            public DateTime DateTimeFormat { get; set; }

            [ExcelDateTimeFormat("HH:mm:ss tt", "HH:MM:SS AM/PM")]
            [ReportVariable(11, nameof(ExcelDateTimeFormat))]
            public DateTime ExcelDateTimeFormat { get; set; }

            [DecimalPrecision(1)]
            [ReportVariable(12, nameof(DecimalPrecision))]
            public decimal DecimalPrecision { get; set; }

            [DecimalPrecision(1, PreserveTrailingZeros = false)]
            [ReportVariable(13, nameof(DecimalPrecisionWithoutTrailingZeros))]
            public decimal DecimalPrecisionWithoutTrailingZeros { get; set; }

            [MaxLength(10)]
            [MaxLength(20, IsHeader = true)]
            [ReportVariable(14, nameof(MaxLength))]
            public string MaxLength { get; set; }

            [MaxLength(10, Text = "...")]
            [MaxLength(20, Text = "...", IsHeader = true)]
            [ReportVariable(15, nameof(MaxLengthWithCustomText))]
            public string MaxLengthWithCustomText { get; set; }

            [PercentFormat(1)]
            [ReportVariable(16, nameof(PercentFormat))]
            public decimal PercentFormat { get; set; }

            [PercentFormat(1, PreserveTrailingZeros = false)]
            [ReportVariable(17, nameof(PercentFormatWithoutTrailingZeros))]
            public decimal PercentFormatWithoutTrailingZeros { get; set; }

            [PercentFormat(1, PostfixText = " (%)")]
            [ReportVariable(18, nameof(PercentFormatWithCustomText))]
            public decimal PercentFormatWithCustomText { get; set; }

            [PercentFormat(1, PreserveTrailingZeros = false, PostfixText = " (%)")]
            [ReportVariable(19, nameof(PercentFormatWithoutTrailingZerosWithCustomText))]
            public decimal PercentFormatWithoutTrailingZerosWithCustomText { get; set; }

            [SameColumnFormat]
            [ReportVariable(20, nameof(SameColumnFormat))]
            public string SameColumnFormat { get; set; }
        }
    }
}
