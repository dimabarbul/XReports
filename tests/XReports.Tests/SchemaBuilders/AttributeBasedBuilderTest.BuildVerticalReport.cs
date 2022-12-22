using System;
using System.Linq;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class AttributeBasedBuilderTest
    {
        [Fact]
        public void BuildVerticalReportShouldAddPropertiesInCorrectOrder()
        {
            AttributeBasedBuilder builderHelper = new(this.serviceProvider);
            IReportSchema<SeveralPropertiesClass> schema = builderHelper.BuildSchema<SeveralPropertiesClass>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<SeveralPropertiesClass>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "ID", "Name" },
            });
        }

        [Fact]
        public void BuildVerticalReportShouldIgnorePropertiesWithoutAttribute()
        {
            AttributeBasedBuilder builderHelper = new(this.serviceProvider);
            IReportSchema<SomePropertiesWithoutAttribute> schema = builderHelper.BuildSchema<SomePropertiesWithoutAttribute>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<SomePropertiesWithoutAttribute>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Name" },
            });
        }

        [Fact]
        public void BuildVerticalReportShouldSetCorrectValueTypeForPropertiesWithAttribute()
        {
            AttributeBasedBuilder builderHelper = new(this.serviceProvider);
            IReportSchema<PropertiesWithAttributes> schema = builderHelper.BuildSchema<PropertiesWithAttributes>();

            PropertiesWithAttributes item = new()
            {
                Id = 1,
                Name = "John Doe",
                Salary = 1000m,
                DateOfBirth = new DateTime(2000, 4, 7),
            };
            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[] { item });

            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { item.Id, item.Name, item.Salary, item.DateOfBirth },
            });
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
