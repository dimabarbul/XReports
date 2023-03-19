using XReports.SchemaBuilders.Attributes;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class HorizontalHeaderRowTest
    {
        [HorizontalReport]
        private class MultiplePropertiesClass
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [HeaderRow]
            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            [ReportColumn(1, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        private class DuplicatedTitle
        {
            [HeaderRow]
            [ReportColumn(1, "Name")]
            public string FirstName { get; set; }

            [HeaderRow]
            [ReportColumn(2, "Name")]
            public string LastName { get; set; }

            [ReportColumn(1, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        private class GapInOrder
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [HeaderRow]
            [ReportColumn(3, "Name")]
            public string Name { get; set; }

            [ReportColumn(1, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        private class DuplicatedOrder
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [HeaderRow]
            [ReportColumn(1, "Name")]
            public string Name { get; set; }

            [ReportColumn(1, "Age")]
            public int Age { get; set; }
        }

        private class Vertical
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [HeaderRow]
            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            [ReportColumn(1, "Age")]
            public int Age { get; set; }
        }
    }
}
