using System;
using System.Linq;
using FluentAssertions;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Extensions.Builders.Tests.BuilderHelpers
{
    public partial class EntityAttributeBuilderHelperTest
    {
        [Fact]
        public void BuildVerticalReport_SeveralProperties_CorrectOrder()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Mocks.ServiceProvider);
            IReportSchema<SeveralPropertiesClass> schema = builderHelper.BuildSchema<SeveralPropertiesClass>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<SeveralPropertiesClass>());

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0].Should().HaveCount(2);
            headerCells[0][0].GetValue<string>().Should().Be("ID");
            headerCells[0][1].GetValue<string>().Should().Be("Name");
        }

        [Fact]
        public void BuildVerticalReport_PropertiesWithoutAttribute_Ignored()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Mocks.ServiceProvider);
            IReportSchema<SomePropertiesWithoutAttribute> schema = builderHelper.BuildSchema<SomePropertiesWithoutAttribute>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<SomePropertiesWithoutAttribute>());

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0].Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("Name");
        }

        [Fact]
        public void BuildVerticalReport_PropertiesWithAttribute_CorrectValueType()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Mocks.ServiceProvider);
            IReportSchema<PropertiesWithAttributes> schema = builderHelper.BuildSchema<PropertiesWithAttributes>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new PropertiesWithAttributes() { Id = 1, Name = "John Doe", Salary = 1000m, DateOfBirth = new DateTime(2000, 4, 7) },
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.Rows);
            headerCells.Should().HaveCount(1);
            headerCells[0].Should().HaveCount(4);
            headerCells[0][0].ValueType.Should().Be(typeof(int));
            headerCells[0][1].ValueType.Should().Be(typeof(string));
            headerCells[0][2].ValueType.Should().Be(typeof(decimal));
            headerCells[0][3].ValueType.Should().Be(typeof(DateTime));
        }

        private class SeveralPropertiesClass
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

            [ReportVariable(2, "DateOfBirth")]
            public DateTime DateOfBirth { get; set; }
        }
    }
}
