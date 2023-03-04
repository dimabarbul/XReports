using System;
using XReports.Attributes;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class ReportVariableTest
    {
        private class MultiplePropertiesClass
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }
        }

        private class SomePropertiesWithoutAttribute
        {
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }
        }

        private class PropertiesWithAttributes
        {
            [ReportVariable(0, "ID")]
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }

            [ReportVariable(2, "Salary")]
            public decimal Salary { get; set; }

            [ReportVariable(3, "DateOfBirth")]
            public DateTime DateOfBirth { get; set; }
        }

        private class DuplicatedTitle
        {
            [ReportVariable(1, "Address")]
            public string Address1 { get; set; }

            [ReportVariable(2, "Address")]
            public string Address2 { get; set; }
        }

        private class GapInOrder
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(3, "Name")]
            public string Name { get; set; }
        }

        private class DuplicatedOrder
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }
        }

        private class NoReportVariables
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
