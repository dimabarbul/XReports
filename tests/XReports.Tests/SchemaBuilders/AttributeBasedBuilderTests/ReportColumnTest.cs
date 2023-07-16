using System;
using System.Linq;
using FluentAssertions;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class ReportColumnTest
    {
        [Fact]
        public void BuildSchemaShouldAddPropertiesInCorrectOrder()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<MultiplePropertiesClass> schema = builderHelper.BuildSchema<MultiplePropertiesClass>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<MultiplePropertiesClass>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell("Name"),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldIgnorePropertiesWithoutAttribute()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<SomePropertiesWithoutAttribute> schema = builderHelper.BuildSchema<SomePropertiesWithoutAttribute>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<SomePropertiesWithoutAttribute>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Name"),
                },
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

            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(item.Id),
                    ReportCellHelper.CreateReportCell(item.Name),
                    ReportCellHelper.CreateReportCell(item.Salary),
                    ReportCellHelper.CreateReportCell(item.DateOfBirth),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldNotThrowWhenTitleIsDuplicated()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<DuplicatedTitle> schema = builderHelper.BuildSchema<DuplicatedTitle>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<DuplicatedTitle>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Address"),
                    ReportCellHelper.CreateReportCell("Address"),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldNotThrowWhenGapInOrder()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<GapInOrder> schema = builderHelper.BuildSchema<GapInOrder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<GapInOrder>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell("Name"),
                },
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
    }
}
