using System;
using XReports.Attributes;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class ReportVariableTest
    {
        private class MultiplePropertiesClass
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }
        }

        private class SomePropertiesWithoutAttribute
        {
            public int Id { get; set; }

            [ReportColumn(1, "Name")]
            public string Name { get; set; }
        }

        private class PropertiesWithAttributes
        {
            [ReportColumn(0, "ID")]
            public int Id { get; set; }

            [ReportColumn(1, "Name")]
            public string Name { get; set; }

            [ReportColumn(2, "Salary")]
            public decimal Salary { get; set; }

            [ReportColumn(3, "DateOfBirth")]
            public DateTime DateOfBirth { get; set; }
        }

        private class DuplicatedTitle
        {
            [ReportColumn(1, "Address")]
            public string Address1 { get; set; }

            [ReportColumn(2, "Address")]
            public string Address2 { get; set; }
        }

        private class GapInOrder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(3, "Name")]
            public string Name { get; set; }
        }

        private class DuplicatedOrder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(1, "Name")]
            public string Name { get; set; }
        }

        private class NoReportVariables
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
