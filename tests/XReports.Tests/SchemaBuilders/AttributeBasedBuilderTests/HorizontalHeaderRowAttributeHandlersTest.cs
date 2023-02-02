using FluentAssertions;
using XReports.AttributeHandlers;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
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
            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { "ID", 1, 2 },
            });
            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Name")
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new BoldProperty(),
                        },
                    },
                    new ReportCellData("John Doe")
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new BoldProperty(),
                        },
                    },
                    new ReportCellData("Jane Doe")
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new BoldProperty(),
                        },
                    },
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
            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    "ID",
                    new ReportCellData(1)
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new BoldProperty(),
                        },
                    },
                    new ReportCellData(2)
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new BoldProperty(),
                        },
                    },
                },
            });
            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "Name", "John Doe", "Jane Doe" },
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
            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("ID")
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new BoldProperty(),
                        },
                    },
                    1,
                    2,
                },
            });
            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "Name", "John Doe", "Jane Doe" },
            });
        }
    }
}
