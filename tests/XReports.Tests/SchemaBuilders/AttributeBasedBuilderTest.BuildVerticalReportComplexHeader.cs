using System.Linq;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class AttributeBasedBuilderTest
    {
        [Fact]
        public void BuildVerticalReportShouldSupportOneComplexHeader()
        {
            AttributeBasedBuilder builderHelper = new(this.serviceProvider);
            IReportSchema<OneComplexHeaderClass> schema = builderHelper.BuildSchema<OneComplexHeaderClass>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<OneComplexHeaderClass>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("ID") { RowSpan = 2 },
                    new ReportCellData("Personal") { ColumnSpan = 2 },
                    null,
                },
                new object[] { null, "Name", "Age" },
            });
        }

        [Fact]
        public void BuildVerticalReportShouldSupportSeveralLevelsOfComplexHeader()
        {
            AttributeBasedBuilder builderHelper = new(this.serviceProvider);
            IReportSchema<SeveralLevelsOfComplexHeaderClass> schema = builderHelper.BuildSchema<SeveralLevelsOfComplexHeaderClass>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<SeveralLevelsOfComplexHeaderClass>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("ID") { RowSpan = 4 },
                    new ReportCellData("Employee Info") { ColumnSpan = 4 },
                    null,
                    null,
                    null,
                    new ReportCellData("Employee # in Department") { RowSpan = 4 },
                },
                new object[]
                {
                    null,
                    new ReportCellData("Personal") { ColumnSpan = 2 },
                    null,
                    new ReportCellData("Job Info") { ColumnSpan = 2 },
                    null,
                    null,
                },
                new object[]
                {
                    null,
                    new ReportCellData("Name") { RowSpan = 2 },
                    new ReportCellData("Age") { RowSpan = 2 },
                    new ReportCellData("Job Title") { RowSpan = 2 },
                    "Sensitive",
                    null,
                },
                new object[]
                {
                    null,
                    null,
                    null,
                    null,
                    "Salary",
                    null,
                },
            });
        }

        private class OneComplexHeaderClass
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name", ComplexHeader = new[] { "Personal" })]
            public string Name { get; set; }

            [ReportVariable(3, "Age", ComplexHeader = new[] { "Personal" })]
            public int Age { get; set; }
        }

        private class SeveralLevelsOfComplexHeaderClass
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name", ComplexHeader = new[] { "Employee Info", "Personal" })]
            public string Name { get; set; }

            [ReportVariable(3, "Age", ComplexHeader = new[] { "Employee Info", "Personal" })]
            public int Age { get; set; }

            [ReportVariable(4, "Job Title", ComplexHeader = new[] { "Employee Info", "Job Info" })]
            public string JobTitle { get; set; }

            [ReportVariable(5, "Salary", ComplexHeader = new[] { "Employee Info", "Job Info", "Sensitive" })]
            public decimal Salary { get; set; }

            [ReportVariable(6, "Employee # in Department")]
            public int DepartmentEmployeeCount { get; set; }
        }
    }
}
