using System.Linq;
using FluentAssertions;
using Reports.Extensions.Builders.Attributes;
using Reports.Extensions.Builders.BuilderHelpers;
using Reports.Interfaces;
using Reports.Models;
using Reports.ReportBuilders;
using Xunit;

namespace Reports.Extensions.Builders.Tests.BuilderHelpers
{
    public partial class EntityAttributeBuilderHelperTest
    {
        [Fact]
        public void BuildVerticalReport_OneComplexHeader_CorrectTable()
        {
            VerticalReportBuilder<OneComplexHeaderClass> builder = new VerticalReportBuilder<OneComplexHeaderClass>();
            EntityAttributeBuilderHelper builderHelper = new EntityAttributeBuilderHelper();
            builderHelper.BuildVerticalReport(builder);

            IReportTable<ReportCell> reportTable = builder.Build(Enumerable.Empty<OneComplexHeaderClass>());

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);
            headerCells.Should().HaveCount(2);
            headerCells[0].Should().HaveCount(3);
            headerCells[0][0].GetValue<string>().Should().Be("ID");
            headerCells[0][1].GetValue<string>().Should().Be("Personal");
            headerCells[0][2].Should().BeNull();
            headerCells[1].Should().HaveCount(3);
            headerCells[1][0].Should().BeNull();
            headerCells[1][1].GetValue<string>().Should().Be("Name");
            headerCells[1][2].GetValue<string>().Should().Be("Age");
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

        [Fact]
        public void BuildVerticalReport_SeveralLevelsOfComplexHeader_CorrectTable()
        {
            VerticalReportBuilder<SeveralLevelsOfComplexHeaderClass> builder = new VerticalReportBuilder<SeveralLevelsOfComplexHeaderClass>();
            EntityAttributeBuilderHelper builderHelper = new EntityAttributeBuilderHelper();
            builderHelper.BuildVerticalReport(builder);

            IReportTable<ReportCell> reportTable = builder.Build(Enumerable.Empty<SeveralLevelsOfComplexHeaderClass>());

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);
            headerCells.Should().HaveCount(4);
            headerCells[0].Should().HaveCount(6);
            headerCells[0][0].GetValue<string>().Should().Be("ID");
            headerCells[0][1].GetValue<string>().Should().Be("Employee Info");
            headerCells[0][2].Should().BeNull();
            headerCells[0][3].Should().BeNull();
            headerCells[0][4].Should().BeNull();
            headerCells[0][5].GetValue<string>().Should().Be("Employee # in Department");
            headerCells[1].Should().HaveCount(6);
            headerCells[1][0].Should().BeNull();
            headerCells[1][1].GetValue<string>().Should().Be("Personal");
            headerCells[1][2].Should().BeNull();
            headerCells[1][3].GetValue<string>().Should().Be("Job Info");
            headerCells[1][4].Should().BeNull();
            headerCells[1][5].Should().BeNull();
            headerCells[2].Should().HaveCount(6);
            headerCells[2][0].Should().BeNull();
            headerCells[2][1].GetValue<string>().Should().Be("Name");
            headerCells[2][2].GetValue<string>().Should().Be("Age");
            headerCells[2][3].GetValue<string>().Should().Be("Job Title");
            headerCells[2][4].GetValue<string>().Should().Be("Sensitive");
            headerCells[2][5].Should().BeNull();
            headerCells[3].Should().HaveCount(6);
            headerCells[3][0].Should().BeNull();
            headerCells[3][1].Should().BeNull();
            headerCells[3][2].Should().BeNull();
            headerCells[3][3].Should().BeNull();
            headerCells[3][4].GetValue<string>().Should().Be("Salary");
            headerCells[3][5].Should().BeNull();
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
