using XReports.Attributes;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class HorizontalHeaderRowAttributeHandlersTest
    {
        private class VerticalWithHeaderRowAttribute
        {
            [HeaderRow(1, "ID")]
            [Custom]
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        private class WithCustomAttribute
        {
            [HeaderRow(1, "ID")]
            [Custom]
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        [Custom]
        private class WithGlobalCustomAttribute
        {
            [HeaderRow(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        [Bold]
        [Bold(IsHeader = true)]
        private class WithGlobalAttribute
        {
            [HeaderRow(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        private class WithCommonAttribute
        {
            [HeaderRow(1, "ID")]
            [Bold]
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        private class WithCommonHeaderAttribute
        {
            [HeaderRow(1, "ID")]
            [Bold(IsHeader = true)]
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }
        }
    }
}
