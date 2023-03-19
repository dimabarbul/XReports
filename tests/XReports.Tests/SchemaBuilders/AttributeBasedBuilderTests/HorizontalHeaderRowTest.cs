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

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell(1),
                    ReportCellHelper.CreateReportCell(2),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("John Doe"),
                    ReportCellHelper.CreateReportCell("Jane Doe"),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Age"),
                    ReportCellHelper.CreateReportCell(22),
                    ReportCellHelper.CreateReportCell(21),
                },
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

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("John"),
                    ReportCellHelper.CreateReportCell("Jane"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("Doe"),
                    ReportCellHelper.CreateReportCell("Doe"),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Age"),
                    ReportCellHelper.CreateReportCell(22),
                    ReportCellHelper.CreateReportCell(21),
                },
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

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell(1),
                    ReportCellHelper.CreateReportCell(2),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("John Doe"),
                    ReportCellHelper.CreateReportCell("Jane Doe"),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Age"),
                    ReportCellHelper.CreateReportCell(22),
                    ReportCellHelper.CreateReportCell(21),
                },
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
        public void BuildSchemaShouldNotThrowWhenReportIsVertical()
        {
            AttributeBasedBuilder builderHelper = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builderHelper.BuildSchema<Vertical>();

            action.Should().NotThrow();
        }
    }
}
