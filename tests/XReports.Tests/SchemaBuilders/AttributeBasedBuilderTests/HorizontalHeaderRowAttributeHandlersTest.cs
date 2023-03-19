using System;
using FluentAssertions;
using XReports.ReportCellProperties;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.AttributeHandlers;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class HorizontalHeaderRowAttributeHandlersTest
    {
        [Fact]
        public void BuildSchemaShouldCallAttributeHandlerForHeaderRowProperties()
        {
            CustomAttributeHandler customAttributeHandler1 = new CustomAttributeHandler();
            CustomAttributeHandler customAttributeHandler2 = new CustomAttributeHandler();
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                customAttributeHandler1,
                customAttributeHandler2,
            });

            IReportSchema<WithCustomAttribute> _ = builder.BuildSchema<WithCustomAttribute>();

            customAttributeHandler1.CallsCount.Should().Be(1);
            customAttributeHandler2.CallsCount.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldNotCallAttributeHandlerForGlobalProperties()
        {
            CustomAttributeHandler customAttributeHandler = new CustomAttributeHandler();
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                customAttributeHandler,
            });

            IReportSchema<WithGlobalCustomAttribute> _ = builder.BuildSchema<WithGlobalCustomAttribute>();

            customAttributeHandler.CallsCount.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldIgnoreGlobalPropertiesForHeaderRows()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                new CommonAttributeHandler(),
            });

            IReportSchema<WithGlobalAttribute> schema = builder.BuildSchema<WithGlobalAttribute>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new WithGlobalAttribute()
                {
                    Id = 1,
                    Name = "John Doe",
                },
                new WithGlobalAttribute()
                {
                    Id = 2,
                    Name = "Jane Doe",
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
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Name", new BoldProperty()),
                    ReportCellHelper.CreateReportCell("John Doe", new BoldProperty()),
                    ReportCellHelper.CreateReportCell("Jane Doe", new BoldProperty()),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldAddCommonPropertiesForHeaderRows()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                new CommonAttributeHandler(),
            });

            IReportSchema<WithCommonAttribute> schema = builder.BuildSchema<WithCommonAttribute>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new WithCommonAttribute()
                {
                    Id = 1,
                    Name = "John Doe",
                },
                new WithCommonAttribute()
                {
                    Id = 2,
                    Name = "Jane Doe",
                },
            });
            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell(1, new BoldProperty()),
                    ReportCellHelper.CreateReportCell(2, new BoldProperty()),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("John Doe"),
                    ReportCellHelper.CreateReportCell("Jane Doe"),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldAddCommonPropertiesForForHeaderForHeaderRows()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                new CommonAttributeHandler(),
            });

            IReportSchema<WithCommonHeaderAttribute> schema = builder.BuildSchema<WithCommonHeaderAttribute>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new WithCommonHeaderAttribute()
                {
                    Id = 1,
                    Name = "John Doe",
                },
                new WithCommonHeaderAttribute()
                {
                    Id = 2,
                    Name = "Jane Doe",
                },
            });
            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", new BoldProperty()),
                    ReportCellHelper.CreateReportCell(1),
                    ReportCellHelper.CreateReportCell(2),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("John Doe"),
                    ReportCellHelper.CreateReportCell("Jane Doe"),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldNotThrowWhenVerticalReportHasHeaderRows()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                new CommonAttributeHandler(),
            });

            Action action = () => builder.BuildSchema<VerticalWithHeaderRowAttribute>();

            action.Should().NotThrow();
        }
    }
}
