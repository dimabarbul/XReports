using XReports.Attributes;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class HorizontalHeaderRowAttributeHandlersTest
    {
        private class VerticalWithHeaderRowAttribute
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            [Custom]
            public int Id { get; set; }

            [ReportColumn(1, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        private class WithCustomAttribute
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            [Custom]
            public int Id { get; set; }

            [ReportColumn(1, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        [Custom]
        private class WithGlobalCustomAttribute
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(1, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        [Bold]
        [Bold(IsHeader = true)]
        private class WithGlobalAttribute
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(1, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        private class WithCommonAttribute
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            [Bold]
            public int Id { get; set; }

            [ReportColumn(1, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        private class WithCommonHeaderAttribute
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            [Bold(IsHeader = true)]
            public int Id { get; set; }

            [ReportColumn(1, "Name")]
            public string Name { get; set; }
        }
    }
}
