using System;
using System.Linq;
using FluentAssertions;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public class ReportVariableTest
    {
        [Fact]
        public void BuildSchemaShouldAddPropertiesInCorrectOrder()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<MultiplePropertiesClass> schema = builderHelper.BuildSchema<MultiplePropertiesClass>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<MultiplePropertiesClass>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "ID", "Name" },
            });
        }

        [Fact]
        public void BuildSchemaShouldIgnorePropertiesWithoutAttribute()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<SomePropertiesWithoutAttribute> schema = builderHelper.BuildSchema<SomePropertiesWithoutAttribute>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<SomePropertiesWithoutAttribute>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Name" },
            });
        }

        [Fact]
        public void BuildSchemaShouldSetCorrectValueTypeForPropertiesWithAttribute()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<PropertiesWithAttributes> schema = builderHelper.BuildSchema<PropertiesWithAttributes>();

            PropertiesWithAttributes item = new PropertiesWithAttributes
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

        [Fact]
        public void BuildSchemaShouldNotThrowWhenTitleIsDuplicated()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<DuplicatedTitle> schema = builderHelper.BuildSchema<DuplicatedTitle>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<DuplicatedTitle>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Address", "Address" },
            });
        }

        [Fact]
        public void BuildSchemaShouldNotThrowWhenGapInOrder()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<GapInOrder> schema = builderHelper.BuildSchema<GapInOrder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<GapInOrder>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "ID", "Name" },
            });
        }

        [Fact]
        public void BuildSchemaShouldThrowWhenOrderIsDuplicated()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builderHelper.BuildSchema<DuplicatedOrder>();

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void BuildSchemaShouldThrowWhenNoPropertyHasReportVariableAttribute()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builderHelper.BuildSchema<NoReportVariables>();

            action.Should().ThrowExactly<ArgumentException>();
        }

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
