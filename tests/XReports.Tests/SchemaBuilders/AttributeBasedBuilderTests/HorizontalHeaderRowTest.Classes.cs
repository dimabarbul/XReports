using XReports.Attributes;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class HorizontalHeaderRowTest
    {
        [HorizontalReport]
        private class MultiplePropertiesClass
        {
            [HeaderRow(1, "ID")]
            public int Id { get; set; }

            [HeaderRow(2, "Name")]
            public string Name { get; set; }

            [ReportVariable(1, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        private class DuplicatedTitle
        {
            [HeaderRow(1, "Name")]
            public string FirstName { get; set; }

            [HeaderRow(2, "Name")]
            public string LastName { get; set; }

            [ReportVariable(1, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        private class GapInOrder
        {
            [HeaderRow(1, "ID")]
            public int Id { get; set; }

            [HeaderRow(3, "Name")]
            public string Name { get; set; }

            [ReportVariable(1, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        private class DuplicatedOrder
        {
            [HeaderRow(1, "ID")]
            public int Id { get; set; }

            [HeaderRow(1, "Name")]
            public string Name { get; set; }

            [ReportVariable(1, "Age")]
            public int Age { get; set; }
        }

        private class Vertical
        {
            [HeaderRow(1, "ID")]
            public int Id { get; set; }

            [HeaderRow(2, "Name")]
            public string Name { get; set; }

            [ReportVariable(1, "Age")]
            public int Age { get; set; }
        }
    }
}
