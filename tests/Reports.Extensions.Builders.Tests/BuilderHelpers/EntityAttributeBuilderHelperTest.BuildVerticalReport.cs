using System;
using System.Collections;
using System.Linq;
using FluentAssertions;
using Reports.Builders;
using Reports.Extensions.Builders.Attributes;
using Reports.Extensions.Builders.BuilderHelpers;
using Reports.Interfaces;
using Reports.Models;
using Xunit;

namespace Reports.Extensions.Builders.Tests.BuilderHelpers
{
    public partial class EntityAttributeBuilderHelperTest
    {
        [Fact]
        public void BuildVerticalReport_SeveralProperties_CorrectOrder()
        {
            VerticalReportBuilder<SeveralPropertiesClass> builder = new VerticalReportBuilder<SeveralPropertiesClass>();
            EntityAttributeBuilderHelper builderHelper = new EntityAttributeBuilderHelper();
            builderHelper.BuildVerticalReport(builder);

            IReportTable<ReportCell> reportTable = builder.Build(Enumerable.Empty<SeveralPropertiesClass>());

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0].Should().HaveCount(2);
            headerCells[0][0].GetValue<string>().Should().Be("ID");
            headerCells[0][1].GetValue<string>().Should().Be("Name");
        }

        private class SeveralPropertiesClass
        {
            [ReportVariable(1, "ID")] public int Id { get; set; }

            [ReportVariable(2, "Name")] public string Name { get; set; }
        }

        [Fact]
        public void BuildVerticalReport_PropertiesWithoutAttribute_Ignored()
        {
            VerticalReportBuilder<SomePropertiesWithoutAttribute> builder = new VerticalReportBuilder<SomePropertiesWithoutAttribute>();
            EntityAttributeBuilderHelper builderHelper = new EntityAttributeBuilderHelper();
            builderHelper.BuildVerticalReport(builder);

            IReportTable<ReportCell> reportTable = builder.Build(Enumerable.Empty<SomePropertiesWithoutAttribute>());

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0].Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("Name");
        }

        private class SomePropertiesWithoutAttribute
        {
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }
        }

        [Fact]
        public void BuildVerticalReport_PropertiesWithAttribute_CorrectValueType()
        {
            VerticalReportBuilder<PropertiesWithAttributes> builder = new VerticalReportBuilder<PropertiesWithAttributes>();
            EntityAttributeBuilderHelper builderHelper = new EntityAttributeBuilderHelper();
            builderHelper.BuildVerticalReport(builder);

            IReportTable<ReportCell> reportTable = builder.Build(new []
            {
                new PropertiesWithAttributes(){ Id = 1, Name = "John Doe", Salary = 1000m, DateOfBirth = new DateTime(2000, 4, 7)},
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.Rows);
            headerCells.Should().HaveCount(1);
            headerCells[0].Should().HaveCount(4);
            headerCells[0][0].ValueType.Should().Be(typeof(int));
            headerCells[0][1].ValueType.Should().Be(typeof(string));
            headerCells[0][2].ValueType.Should().Be(typeof(decimal));
            headerCells[0][3].ValueType.Should().Be(typeof(DateTime));
        }

        private class PropertiesWithAttributes
        {
            [ReportVariable(0, "ID")]
            public int Id { get; set; }

            [ReportVariable(1, "Name")]
            public string Name { get; set; }

            [ReportVariable(2, "Salary")]
            public decimal Salary { get; set; }

            [ReportVariable(2, "DateOfBirth")]
            public DateTime DateOfBirth { get; set; }
        }
    }
}