using System;
using System.Linq;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class HorizontalHeaderRowTest
    {
        [Fact]
        public void BuildSchemaShouldAddHeaderRowsInCorrectOrder()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<MultiplePropertiesClass> schema = builderHelper.BuildSchema<MultiplePropertiesClass>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new MultiplePropertiesClass()
                {
                    Id = 1,
                    Name = "John Doe",
                    Age = 22,
                },
                new MultiplePropertiesClass()
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Age = 21,
                },
            });

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { "ID", 1, 2 },
                new object[] { "Name", "John Doe", "Jane Doe" },
            });
            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "Age", 22, 21 },
            });
        }

        [Fact]
        public void BuildSchemaShouldNotThrowWhenHeaderRowTitleIsDuplicated()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<DuplicatedTitle> schema = builderHelper.BuildSchema<DuplicatedTitle>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new DuplicatedTitle()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Age = 22,
                },
                new DuplicatedTitle()
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Age = 21,
                },
            });

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Name", "John", "Jane" },
                new[] { "Name", "Doe", "Doe" },
            });
            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "Age", 22, 21 },
            });
        }

        [Fact]
        public void BuildSchemaShouldNotThrowWhenGapInHeaderRowsOrder()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<GapInOrder> schema = builderHelper.BuildSchema<GapInOrder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new GapInOrder()
                {
                    Id = 1,
                    Name = "John Doe",
                    Age = 22,
                },
                new GapInOrder()
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Age = 21,
                },
            });

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { "ID", 1, 2 },
                new object[] { "Name", "John Doe", "Jane Doe" },
            });
            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "Age", 22, 21 },
            });
        }

        [Fact]
        public void BuildSchemaShouldThrowWhenHeaderRowsOrderIsDuplicated()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builderHelper.BuildSchema<DuplicatedOrder>();

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void BuildSchemaShouldThrowWhenReportIsVertical()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builderHelper.BuildSchema<Vertical>();

            action.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}
