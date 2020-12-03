using System;
using System.Linq;
using FluentAssertions;
using Reports.Extensions.AttributeBasedBuilder.Attributes;
using Reports.Interfaces;
using Reports.Models;
using Reports.SchemaBuilders;
using Xunit;

namespace Reports.Extensions.Builders.Tests.BuilderHelpers
{
    public partial class EntityAttributeBuilderHelperTest
    {
        [Fact]
        public void BuildVerticalReport_SeveralProperties_CorrectOrder()
        {
            AttributeBasedBuilder.AttributeBasedBuilder builderHelper = new AttributeBasedBuilder.AttributeBasedBuilder();
            VerticalReportSchemaBuilder<SeveralPropertiesClass> builder = builderHelper.BuildVerticalReport<SeveralPropertiesClass>();

            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(Enumerable.Empty<SeveralPropertiesClass>());

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0].Should().HaveCount(2);
            headerCells[0][0].GetValue<string>().Should().Be("ID");
            headerCells[0][1].GetValue<string>().Should().Be("Name");
        }

        private class SeveralPropertiesClass
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }
        }

        [Fact]
        public void BuildVerticalReport_PropertiesWithoutAttribute_Ignored()
        {
            AttributeBasedBuilder.AttributeBasedBuilder builderHelper = new AttributeBasedBuilder.AttributeBasedBuilder();
            VerticalReportSchemaBuilder<SomePropertiesWithoutAttribute> builder = builderHelper.BuildVerticalReport<SomePropertiesWithoutAttribute>();

            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(Enumerable.Empty<SomePropertiesWithoutAttribute>());

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
            AttributeBasedBuilder.AttributeBasedBuilder builderHelper = new AttributeBasedBuilder.AttributeBasedBuilder();
            VerticalReportSchemaBuilder<PropertiesWithAttributes> builder = builderHelper.BuildVerticalReport<PropertiesWithAttributes>();

            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(new []
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
